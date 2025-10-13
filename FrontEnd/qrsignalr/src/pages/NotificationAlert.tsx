import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

const Dashboard: React.FC = () => {
  interface LocationMap {
    lat: number;
    lng: number;
  }

  const [notifications, setNotifications] = useState<string[]>([]);
  const [qrValue, setQrValue] = useState<string>("");

  const [adminActivity, setAdminActivity] = useState<string[]>([]);

  useEffect(() => {
    //Production
    // const connection = new signalR.HubConnectionBuilder()
    //   .withUrl("https://location.finnetra.com/notificationHub") // backend hub URL
    //   .withAutomaticReconnect()
    //   .build();

    //Development
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7113/notificationHub") // backend hub URL
      .withAutomaticReconnect()
      .build();

    // Handle purchase notifications
    connection.on("ReceivePurchaseNotification", (message: string) => {
      console.log("📢 New purchase notification:", message);
      setNotifications((prev) => [...prev, message]);
    });

    // Handle Admin Activities
    connection.on("AdminActivity", (activitymsg: string) => {
      console.log("📢 New Admin notification:", activitymsg);
      setAdminActivity((actMsg) => [...actMsg, activitymsg]);
    });

    // Handle new QR code
    connection.on("ReceiveNewQr", (newQr: string) => {
      console.log("📷 New QR received:", newQr);
      setQrValue(newQr);
    });

    connection
      .start()
      .then(() => console.log("✅ SignalR connected"))
      .catch((err) => console.error("❌ SignalR connection error:", err));

    return () => {
      connection.stop();
    };
  }, []);

  return (
    <div style={{ padding: "20px", fontFamily: "Arial" }}>
      <h2>📢 Purchase Notifications</h2>
      {notifications.length === 0 ? (
        <p>No purchase yet...</p>
      ) : (
        <ul>
          {notifications.map((msg, index) => (
            <li key={index}>{msg}</li>
          ))}
        </ul>
      )}

      <h2>📢 AdminActivity Notification</h2>
      {adminActivity.length === 0 ? (
        <p>No activity yet...</p>
      ) : (
        <ul>
          {adminActivity.map((actmsg, index) => (
            <li key={index}>{actmsg}</li>
          ))}
        </ul>
      )}
      <h3>📷 Scan QR Code</h3>
      {qrValue ? (
        <img
          src={`https://location.finnetra.com${qrValue}`} // backend sends full relative path like "/qrcodes/abc.png"
          alt="QR Code"
          style={{
            width: "500px",
            height: "500px",
            border: "2px solid #333",
            borderRadius: "8px",
            marginTop: "10px",
          }}
        />
      ) : (
        <p>Waiting for QR code...</p>
      )}
    </div>
  );
};

export default Dashboard;
