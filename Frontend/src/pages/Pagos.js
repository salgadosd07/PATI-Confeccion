import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { pagosApi, talleresApi } from '../services/api';

function Pagos() {
  const [pagos, setPagos] = useState([]);
  const [pagosPendientes, setPagosPendientes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtro, setFiltro] = useState('todos'); // todos, pendientes, pagados

  useEffect(() => {
    loadPagosPendientes();
  }, []);

  const loadPagosPendientes = async () => {
    try {
      const response = await pagosApi.getPendientes();
      setPagosPendientes(response.data);
    } catch (error) {
      console.error('Error cargando pagos pendientes:', error);
    } finally {
      setLoading(false);
    }
  };

  const getEstadoBadge = (estado) => {
    const badges = {
      'Pagado': 'badge-success',
      'Pendiente': 'badge-danger',
      'Parcial': 'badge-warning',
      'En revisión': 'badge-info',
    };
    return badges[estado] || 'badge-secondary';
  };

  const formatCurrency = (value) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
    }).format(value);
  };

  if (loading) return <div>Cargando...</div>;

  const pagosFiltrados = filtro === 'pendientes' 
    ? pagosPendientes.filter(p => p.estadoPago === 'Pendiente')
    : filtro === 'pagados'
    ? pagosPendientes.filter(p => p.estadoPago === 'Pagado')
    : pagosPendientes;

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <h2>Gestión de Pagos</h2>
        <Link to="/pagos/crear" className="btn btn-primary">
          Registrar Pago
        </Link>
      </div>

      <div className="card" style={{ marginBottom: '20px' }}>
        <div style={{ padding: '15px' }}>
          <label style={{ marginRight: '10px' }}>Filtrar por estado:</label>
          <select value={filtro} onChange={(e) => setFiltro(e.target.value)} className="form-control" style={{ width: '200px', display: 'inline-block' }}>
            <option value="todos">Todos</option>
            <option value="pendientes">Pendientes</option>
            <option value="pagados">Pagados</option>
          </select>
        </div>
      </div>

      <div className="card">
        <table className="table">
          <thead>
            <tr>
              <th>Número Pago</th>
              <th>Taller</th>
              <th>Referencia</th>
              <th>Fecha</th>
              <th>Monto Total</th>
              <th>Monto Pagado</th>
              <th>Estado</th>
              <th>Método Pago</th>
              <th>Días Mora</th>
            </tr>
          </thead>
          <tbody>
            {pagosFiltrados.map(pago => (
              <tr key={pago.id}>
                <td><strong>{pago.numeroPago}</strong></td>
                <td>{pago.tallerNombre}</td>
                <td>{pago.referenciaNombre}</td>
                <td>{new Date(pago.fechaPago).toLocaleDateString()}</td>
                <td>{formatCurrency(pago.montoTotal)}</td>
                <td>{pago.montoPagado ? formatCurrency(pago.montoPagado) : '-'}</td>
                <td>
                  <span className={`badge ${getEstadoBadge(pago.estadoPago)}`}>
                    {pago.estadoPago}
                  </span>
                </td>
                <td>{pago.metodoPago || '-'}</td>
                <td>
                  {pago.diasMora > 0 ? (
                    <span className="badge badge-danger">{pago.diasMora} días</span>
                  ) : '-'}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className="card" style={{ marginTop: '20px' }}>
        <div style={{ padding: '20px' }}>
          <h4>Resumen Financiero</h4>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: '20px', marginTop: '15px' }}>
            <div className="stat-card">
              <h5>Total Pendiente</h5>
              <p className="stat-value">
                {formatCurrency(pagosPendientes.filter(p => p.estadoPago === 'Pendiente').reduce((sum, p) => sum + p.montoTotal, 0))}
              </p>
            </div>
            <div className="stat-card">
              <h5>Total Pagado</h5>
              <p className="stat-value">
                {formatCurrency(pagosPendientes.filter(p => p.estadoPago === 'Pagado').reduce((sum, p) => sum + (p.montoPagado || 0), 0))}
              </p>
            </div>
            <div className="stat-card">
              <h5>Pagos con Mora</h5>
              <p className="stat-value">
                {pagosPendientes.filter(p => p.diasMora > 0).length}
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Pagos;
