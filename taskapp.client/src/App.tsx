import { Routes, Route } from "react-router-dom";
import { DashboardLayout } from "./layouts/DashboardLayout";
import { Welcome } from "./pages/Welcome/Welcome";
import { Login } from "./pages/auth/login";
import { Register } from "./pages/auth/register";

function AppRoutes() {
  return (
    <Routes>
      <Route path="/" element={<Welcome />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />

      {/* Dashboard routes */}
      <Route path="/taskly/*" element={<DashboardLayout />} />
    </Routes>
  );
}

export default AppRoutes;
