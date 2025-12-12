import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import Layout from './components/Layout/Layout';
import ProtectedRoute from './components/ProtectedRoute';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import Cortes from './pages/Cortes';
import CrearCorte from './pages/CrearCorte';
import Asignaciones from './pages/Asignaciones';
import CrearAsignacion from './pages/CrearAsignacion';
import Talleres from './pages/Talleres';
import Referencias from './pages/Referencias';
import Remisiones from './pages/Remisiones';
import CrearRemision from './pages/CrearRemision';
import CrearControlCalidad from './pages/CrearControlCalidad';
import Pagos from './pages/Pagos';
import CrearPago from './pages/CrearPago';
import Inventario from './pages/Inventario';
import Reportes from './pages/Reportes';
import RegistrarAvance from './pages/RegistrarAvance';
import Archivos from './pages/Archivos';
import Unauthorized from './pages/Unauthorized';
import './App.css';

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/unauthorized" element={<Unauthorized />} />
          <Route path="/" element={<Layout />}>
            <Route index element={<Navigate to="/dashboard" replace />} />
            
            {/* Dashboard - Solo Administrador y Financiero */}
            <Route path="dashboard" element={
              <ProtectedRoute roles={['Administrador', 'Financiero']}>
                <Dashboard />
              </ProtectedRoute>
            } />
            
            {/* Cortes - Administrador, Corte, Taller, Calidad, Bodega */}
            <Route path="cortes" element={
              <ProtectedRoute roles={['Administrador', 'Corte', 'Taller', 'Calidad', 'Bodega']}>
                <Cortes />
              </ProtectedRoute>
            } />
            
            {/* Crear Corte - Solo Administrador y Corte */}
            <Route path="cortes/crear" element={
              <ProtectedRoute roles={['Administrador', 'Corte']}>
                <CrearCorte />
              </ProtectedRoute>
            } />
            
            {/* Asignaciones - Todos los autenticados */}
            <Route path="asignaciones" element={
              <ProtectedRoute>
                <Asignaciones />
              </ProtectedRoute>
            } />
            
            {/* Crear Asignación - Solo Administrador */}
            <Route path="asignaciones/crear" element={
              <ProtectedRoute roles={['Administrador']}>
                <CrearAsignacion />
              </ProtectedRoute>
            } />
            
            {/* Registrar Avance - Taller */}
            <Route path="asignaciones/:asignacionId/avance" element={
              <ProtectedRoute roles={['Administrador', 'Taller']}>
                <RegistrarAvance />
              </ProtectedRoute>
            } />
            
            {/* Remisiones - Bodega */}
            <Route path="remisiones" element={
              <ProtectedRoute roles={['Administrador', 'Bodega']}>
                <Remisiones />
              </ProtectedRoute>
            } />
            
            <Route path="remisiones/crear" element={
              <ProtectedRoute roles={['Administrador', 'Bodega']}>
                <CrearRemision />
              </ProtectedRoute>
            } />
            
            {/* Control Calidad - Solo Calidad */}
            <Route path="control-calidad/crear" element={
              <ProtectedRoute roles={['Administrador', 'Calidad']}>
                <CrearControlCalidad />
              </ProtectedRoute>
            } />
            
            {/* Pagos - Solo Financiero */}
            <Route path="pagos" element={
              <ProtectedRoute roles={['Administrador', 'Financiero']}>
                <Pagos />
              </ProtectedRoute>
            } />
            
            <Route path="pagos/crear" element={
              <ProtectedRoute roles={['Administrador', 'Financiero']}>
                <CrearPago />
              </ProtectedRoute>
            } />
            
            {/* Inventario - Bodega */}
            <Route path="inventario" element={
              <ProtectedRoute roles={['Administrador', 'Bodega']}>
                <Inventario />
              </ProtectedRoute>
            } />
            
            {/* Reportes - Administrador */}
            <Route path="reportes" element={
              <ProtectedRoute roles={['Administrador']}>
                <Reportes />
              </ProtectedRoute>
            } />
            
            {/* Archivos - Todos */}
            <Route path="archivos" element={
              <ProtectedRoute>
                <Archivos />
              </ProtectedRoute>
            } />
            
            {/* Catálogos - Todos pueden ver */}
            <Route path="talleres" element={
              <ProtectedRoute>
                <Talleres />
              </ProtectedRoute>
            } />
            
            <Route path="referencias" element={
              <ProtectedRoute>
                <Referencias />
              </ProtectedRoute>
            } />
          </Route>
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;
