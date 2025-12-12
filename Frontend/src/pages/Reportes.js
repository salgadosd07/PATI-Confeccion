import React, { useState, useEffect } from 'react';
import { reportesApi, referenciasApi, talleresApi } from '../services/api';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

function Reportes() {
  const [tipoReporte, setTipoReporte] = useState('referencia');
  const [referencias, setReferencias] = useState([]);
  const [talleres, setTalleres] = useState([]);
  const [referenciaSeleccionada, setReferenciaSeleccionada] = useState('');
  const [tallerSeleccionado, setTallerSeleccionado] = useState('');
  const [reporteData, setReporteData] = useState(null);
  const [reporteColores, setReporteColores] = useState([]);
  const [loading, setLoading] = useState(false);
  const [loadingInitial, setLoadingInitial] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    loadOptions();
    loadReporteColores();
  }, []);

  const loadOptions = async () => {
    try {
      const [refResponse, talResponse] = await Promise.all([
        referenciasApi.getAll(),
        talleresApi.getAll()
      ]);
      setReferencias(refResponse.data || []);
      setTalleres(talResponse.data || []);
    } catch (error) {
      console.error('Error cargando opciones:', error);
      setError('Error al cargar referencias y talleres');
    } finally {
      setLoadingInitial(false);
    }
  };

  const loadReporteColores = async () => {
    try {
      const response = await reportesApi.getColores();
      setReporteColores(response.data || []);
    } catch (error) {
      console.error('Error cargando reporte de colores:', error);
      // No mostramos error aquí porque es opcional
    }
  };

  const generarReporte = async () => {
    setLoading(true);
    setError(null);
    
    try {
      let response;
      if (tipoReporte === 'referencia' && referenciaSeleccionada) {
        response = await reportesApi.getPorReferencia(referenciaSeleccionada);
      } else if (tipoReporte === 'taller' && tallerSeleccionado) {
        response = await reportesApi.getPorTaller(tallerSeleccionado);
      } else if (tipoReporte === 'financiero') {
        response = await reportesApi.getFinanciero();
      } else {
        setError('Por favor seleccione una opción válida');
        setLoading(false);
        return;
      }
      setReporteData(response?.data);
      
      // Recargar el reporte de colores después de generar un reporte
      loadReporteColores();
    } catch (error) {
      console.error('Error generando reporte:', error);
      setError('Error al generar el reporte: ' + (error.response?.data?.message || error.message));
    } finally {
      setLoading(false);
    }
  };

  const formatCurrency = (value) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
    }).format(value);
  };

  if (loadingInitial) {
    return <div>Cargando opciones...</div>;
  }

  return (
    <div>
      <h2>Reportes y Análisis</h2>

      {error && (
        <div className="alert alert-danger" style={{ marginBottom: '20px' }}>
          {error}
        </div>
      )}

      <div className="card" style={{ marginBottom: '20px', padding: '20px' }}>
        <h4>Generar Reporte</h4>
        <div style={{ display: 'flex', gap: '15px', alignItems: 'flex-end', marginTop: '15px' }}>
          <div>
            <label>Tipo de Reporte:</label>
            <select 
              value={tipoReporte} 
              onChange={(e) => setTipoReporte(e.target.value)}
              className="form-control"
            >
              <option value="referencia">Por Referencia</option>
              <option value="taller">Por Taller</option>
              <option value="financiero">Financiero</option>
            </select>
          </div>

          {tipoReporte === 'referencia' && (
            <div>
              <label>Seleccionar Referencia:</label>
              <select 
                value={referenciaSeleccionada} 
                onChange={(e) => setReferenciaSeleccionada(e.target.value)}
                className="form-control"
              >
                <option value="">Seleccione...</option>
                {referencias.map(ref => (
                  <option key={ref.id} value={ref.id}>{ref.nombre}</option>
                ))}
              </select>
            </div>
          )}

          {tipoReporte === 'taller' && (
            <div>
              <label>Seleccionar Taller:</label>
              <select 
                value={tallerSeleccionado} 
                onChange={(e) => setTallerSeleccionado(e.target.value)}
                className="form-control"
              >
                <option value="">Seleccione...</option>
                {talleres.map(tal => (
                  <option key={tal.id} value={tal.id}>{tal.nombre}</option>
                ))}
              </select>
            </div>
          )}

          <button onClick={generarReporte} disabled={loading} className="btn btn-primary">
            {loading ? 'Generando...' : 'Generar Reporte'}
          </button>
        </div>
      </div>

      {reporteData && tipoReporte === 'referencia' && (
        <div className="card" style={{ padding: '20px' }}>
          <h4>Reporte: {reporteData.referenciaNombre}</h4>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4, 1fr)', gap: '15px', marginTop: '15px' }}>
            <div className="stat-card">
              <h5>Programada</h5>
              <p className="stat-value">{reporteData.cantidadTotalProgramada}</p>
            </div>
            <div className="stat-card">
              <h5>Cortada</h5>
              <p className="stat-value">{reporteData.cantidadTotalCortada}</p>
            </div>
            <div className="stat-card">
              <h5>Terminada</h5>
              <p className="stat-value">{reporteData.cantidadTerminada}</p>
            </div>
            <div className="stat-card">
              <h5>Imperfectos</h5>
              <p className="stat-value">{reporteData.totalImperfectos}</p>
            </div>
          </div>
        </div>
      )}

      {reporteData && tipoReporte === 'taller' && (
        <div className="card" style={{ padding: '20px' }}>
          <h4>Reporte: {reporteData.tallerNombre}</h4>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4, 1fr)', gap: '15px', marginTop: '15px' }}>
            <div className="stat-card">
              <h5>Asignaciones</h5>
              <p className="stat-value">{reporteData.totalAsignaciones}</p>
            </div>
            <div className="stat-card">
              <h5>Producción Total</h5>
              <p className="stat-value">{reporteData.produccionTotal}</p>
            </div>
            <div className="stat-card">
              <h5>Rendimiento</h5>
              <p className="stat-value">{reporteData.rendimientoPromedio.toFixed(1)}%</p>
            </div>
            <div className="stat-card">
              <h5>Total Pagado</h5>
              <p className="stat-value">{formatCurrency(reporteData.totalPagado)}</p>
            </div>
          </div>
        </div>
      )}

      {reporteData && tipoReporte === 'financiero' && (
        <div className="card" style={{ padding: '20px' }}>
          <h4>Reporte Financiero</h4>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4, 1fr)', gap: '15px', marginTop: '15px' }}>
            <div className="stat-card">
              <h5>Total Programado</h5>
              <p className="stat-value">{formatCurrency(reporteData.totalPagosProgramados)}</p>
            </div>
            <div className="stat-card">
              <h5>Total Pagado</h5>
              <p className="stat-value">{formatCurrency(reporteData.totalPagosPagados)}</p>
            </div>
            <div className="stat-card">
              <h5>Total Pendiente</h5>
              <p className="stat-value">{formatCurrency(reporteData.totalPagosPendientes)}</p>
            </div>
            <div className="stat-card">
              <h5>Costo Promedio/Unidad</h5>
              <p className="stat-value">{formatCurrency(reporteData.costoPromedioUnidad)}</p>
            </div>
          </div>
          
          {reporteData.pagosPorTaller && reporteData.pagosPorTaller.length > 0 && (
            <div style={{ marginTop: '20px' }}>
              <h5>Pagos por Taller</h5>
              <table className="table">
                <thead>
                  <tr>
                    <th>Taller</th>
                    <th>Total Pagado</th>
                    <th>Total Pendiente</th>
                  </tr>
                </thead>
                <tbody>
                  {reporteData.pagosPorTaller.map((pt, index) => (
                    <tr key={index}>
                      <td>{pt.tallerNombre}</td>
                      <td>{formatCurrency(pt.totalPagado)}</td>
                      <td>{formatCurrency(pt.totalPendiente)}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      )}

      <div className="card" style={{ marginTop: '20px', padding: '20px' }}>
        <h4>Distribución por Colores</h4>
        {reporteColores && reporteColores.length > 0 ? (
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={reporteColores.slice(0, 10)}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="colorNombre" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Bar dataKey="cantidadTotal" fill="#8884d8" name="Total Cortado" />
              <Bar dataKey="cantidadDisponible" fill="#82ca9d" name="Disponible" />
            </BarChart>
          </ResponsiveContainer>
        ) : (
          <div className="alert alert-info">
            No hay datos de colores disponibles. Cree algunos cortes primero para ver la distribución por colores.
          </div>
        )}
      </div>
    </div>
  );
}

export default Reportes;
