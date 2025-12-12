import React from 'react';
import { Outlet, Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import './Layout.css';

function Layout() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  if (!user) {
    navigate('/login');
    return null;
  }

  return (
    <div className="layout">
      <nav className="sidebar">
        <div className="sidebar-header">
          <h2>PATI</h2>
          <p>Control de Confección</p>
        </div>
        <ul className="nav-menu">
          <li>
            <Link to="/dashboard">📊 Dashboard</Link>
          </li>
          <li>
            <Link to="/cortes">✂️ Cortes</Link>
          </li>
          <li>
            <Link to="/asignaciones">📋 Asignaciones</Link>
          </li>
          <li>
            <Link to="/remisiones">📦 Remisiones</Link>
          </li>
          <li>
            <Link to="/inventario">📦 Inventario</Link>
          </li>
          <li>
            <Link to="/pagos">💰 Pagos</Link>
          </li>
          <li>
            <Link to="/reportes">📈 Reportes</Link>
          </li>
          <li>
            <Link to="/archivos">📁 Archivos</Link>
          </li>
          <li>
            <Link to="/talleres">🏭 Talleres</Link>
          </li>
          <li>
            <Link to="/referencias">👕 Referencias</Link>
          </li>
        </ul>
      </nav>
      <div className="main-container">
        <header className="header">
          <div className="header-content">
            <h1>Sistema de Gestión de Producción</h1>
            <div className="user-info">
              <span>{user?.nombreCompleto || user?.email}</span>
              <button onClick={handleLogout} className="btn btn-danger">
                Cerrar Sesión
              </button>
            </div>
          </div>
        </header>
        <main className="content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}

export default Layout;
