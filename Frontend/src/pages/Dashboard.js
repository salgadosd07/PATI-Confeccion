import React, { useState, useEffect } from 'react';
import { dashboardApi } from '../services/api';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';
import './Dashboard.css';

function Dashboard() {
  const [dashboardData, setDashboardData] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboard();
  }, []);

  const loadDashboard = async () => {
    try {
      const response = await dashboardApi.getData();
      setDashboardData(response.data);
    } catch (error) {
      console.error('Error cargando dashboard:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div>Cargando...</div>;

  const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042'];

  console.log(dashboardData);

  return (
    <div className="dashboard">
      <h2>Dashboard de Producción</h2>
      
      <div className="stats-grid">
        <div className="stat-card">
          <h3>Total Cortes</h3>
          <p className="stat-value">{dashboardData?.resumenProduccion?.totalCortes || 0}</p>
        </div>
        <div className="stat-card">
          <h3>Prendas Programadas</h3>
          <p className="stat-value">{dashboardData?.resumenProduccion?.totalPrendasProgramadas || 0}</p>
        </div>
        <div className="stat-card">
          <h3>En Proceso</h3>
          <p className="stat-value">{dashboardData?.resumenProduccion?.prendasEnProceso || 0}</p>
        </div>
        <div className="stat-card">
          <h3>Terminadas</h3>
          <p className="stat-value">{dashboardData?.resumenProduccion?.prendasTerminadas || 0}</p>
        </div>
      </div>

      <div className="charts-container">
        <div className="card">
          <h3>Avance por Taller</h3>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={dashboardData?.avancesPorTaller || []}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="tallerNombre" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Bar dataKey="cantidadAsignada" fill="#8884d8" name="Asignada" />
              <Bar dataKey="cantidadLista" fill="#82ca9d" name="Lista" />
            </BarChart>
          </ResponsiveContainer>
        </div>

        <div className="card">
          <h3>Avance por Referencia</h3>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={dashboardData?.avancesPorReferencia || []}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="referenciaNombre" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Bar dataKey="cantidadProgramada" fill="#8884d8" name="Programada" />
              <Bar dataKey="cantidadTerminada" fill="#82ca9d" name="Terminada" />
            </BarChart>
          </ResponsiveContainer>
        </div>
      </div>

      {dashboardData?.alertas && dashboardData.alertas.length > 0 && (
        <div className="card">
          <h3>Alertas</h3>
          {dashboardData.alertas.map((alerta, index) => (
            <div key={index} className="alert alert-warning">
              <strong>{alerta.tipo}:</strong> {alerta.mensaje}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default Dashboard;
