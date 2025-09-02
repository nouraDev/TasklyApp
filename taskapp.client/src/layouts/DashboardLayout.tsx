import { Routes, Route, Navigate } from "react-router-dom";
import { Sidebar } from "../components/sidebar";
import { useState } from "react";
import { UpcomingPage } from "../pages/dashboard/upcomingPage";
import EisenhowerMatrix from "../pages/dashboard/eisenhower-matrix";
import ArchivePage from "../pages/dashboard/archive";
import TodayPage from "../pages/dashboard/todayPage";


export function DashboardLayout() {
  const [lists, setLists] = useState<any[]>([]);
  const [, setTodayCount] = useState(0);

  return (
    <div className="flex min-h-screen bg-gray-100 dark:bg-gray-900 overflow-hidden">
      <Sidebar
        onListsLoaded={setLists}
      />

      <main className="flex-1 p-6 ml-1 overflow-hidden">
        <Routes>
          <Route index element={<Navigate to="today" replace />} />
          <Route
            path="today"
            element={
              <TodayPage
                lists={lists}
                onTaskCountChange={(count) => setTodayCount(count)}
              />
            }
          />
          <Route path="upcoming" element={<UpcomingPage lists={lists} />} />
          <Route path="eisenhower-matrix" element={<EisenhowerMatrix />} />
          <Route path="archive" element={<ArchivePage />} />
          <Route path="settings" element={<div> Settings Page</div>} />
        </Routes>
      </main>
    </div>
  );
}
