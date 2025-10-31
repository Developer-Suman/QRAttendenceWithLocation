import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import LocationMap from "./pages/LocationMap";
import PurchaseNotify from "./pages/NotificationAlert";

const App: React.FC = () => {
  return (
    <Router>
      <nav>
        <Link to="/">Notification</Link> | <Link to="/location">Map</Link>
      </nav>
      <Routes>
        <Route path="/" element={<PurchaseNotify />} />
        <Route path="/location" element={<LocationMap />} />
                <Route path="/location" element={<LocationMap />} />
      </Routes>
    </Router>
  );
};

export default App;
