import React, { useEffect, useState } from "react";
import { MapContainer, TileLayer, Marker, Popup, useMap } from "react-leaflet";
import "leaflet/dist/leaflet.css";
import L from "leaflet";

// Fix default Leaflet marker icons
delete (L.Icon.Default.prototype as any)._getIconUrl;
L.Icon.Default.mergeOptions({
  iconRetinaUrl:
    "https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-icon-2x.png",
  iconUrl:
    "https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-icon.png",
  shadowUrl:
    "https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-shadow.png",
});

interface Location {
  lat: number;
  lng: number;
}

interface Props {
  location?: Location | null; // optional prop
}

// 🔄 Helper component: recenter map when location changes
const RecenterMap: React.FC<{ location: Location }> = ({ location }) => {
  const map = useMap();
  useEffect(() => {
    map.setView([location.lat, location.lng], map.getZoom(), {
      animate: true,
    });
  }, [location, map]);
  return null;
};

const LocationMap: React.FC<Props> = ({ location }) => {
  const defaultLocation: Location = {
    lat: 26.7474416,
    lng: 87.3972815,
  };

  const currentLocation = location || defaultLocation;
  const [address, setAddress] = useState<string>("Fetching address...");
  const [publicIp, setPublicIp] = useState<string>("Fetching IP...");

  // Reverse geocoding (lat/lng → address)
  useEffect(() => {
    const fetchAddress = async () => {
      try {
        const response = await fetch(
          `https://nominatim.openstreetmap.org/reverse?format=json&lat=${currentLocation.lat}&lon=${currentLocation.lng}`
        );
        const data = await response.json();
        setAddress(data.display_name || "Address not found");
      } catch (err) {
        console.error(err);
        setAddress("Error fetching address");
      }
    };

    fetchAddress();
  }, [currentLocation]);

  // Fetch Wi-Fi public IP
  useEffect(() => {
    const fetchPublicIp = async () => {
      try {
        const response = await fetch("https://api.ipify.org?format=json");
        const data = await response.json();
        setPublicIp(data.ip);
      } catch (err) {
        console.error(err);
        setPublicIp("Error fetching IP");
      }
    };

    fetchPublicIp();
  }, []);

  return (
    <>
      <div style={{ position: "absolute", top: 10, left: 10, zIndex: 1000, background: "white", padding: "8px", borderRadius: "5px" }}>
        <b>Public IP:</b> {publicIp}
      </div>
      <MapContainer
        center={[currentLocation.lat, currentLocation.lng]}
        zoom={17}
        style={{ height: "100vh", width: "100vw" }}
      >
        <TileLayer
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          attribution="&copy; OpenStreetMap contributors"
        />
        <Marker position={[currentLocation.lat, currentLocation.lng]}>
          <Popup>
            <b>📍 Location Details</b> <br />
            {address} <br />
            <b>Lat:</b> {currentLocation.lat}, <b>Lng:</b> {currentLocation.lng} <br />
            <b>Public IP:</b> {publicIp}
          </Popup>
        </Marker>
        <RecenterMap location={currentLocation} />
      </MapContainer>
    </>
  );
};

export default LocationMap;
