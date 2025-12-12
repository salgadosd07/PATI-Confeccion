import React, { useState, useEffect } from 'react';
import { inventarioApi, referenciasApi } from '../services/api';

function Inventario() {
  const [inventario, setInventario] = useState([]);
  const [referencias, setReferencias] = useState([]);
  const [filtroReferencia, setFiltroReferencia] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [invResponse, refResponse] = await Promise.all([
        inventarioApi.getDisponible(),
        referenciasApi.getAll()
      ]);
      setInventario(invResponse.data);
      setReferencias(refResponse.data);
    } catch (error) {
      console.error('Error cargando inventario:', error);
    } finally {
      setLoading(false);
    }
  };

  const inventarioFiltrado = filtroReferencia
    ? inventario.filter(item => item.referenciaId === parseInt(filtroReferencia))
    : inventario;

  const totalDisponible = inventarioFiltrado.reduce((sum, item) => sum + item.cantidadDisponible, 0);

  if (loading) return <div>Cargando...</div>;

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <h2>Inventario Terminado</h2>
      </div>

      <div className="card" style={{ marginBottom: '20px' }}>
        <div style={{ padding: '15px' }}>
          <label style={{ marginRight: '10px' }}>Filtrar por referencia:</label>
          <select 
            value={filtroReferencia} 
            onChange={(e) => setFiltroReferencia(e.target.value)} 
            className="form-control" 
            style={{ width: '250px', display: 'inline-block' }}
          >
            <option value="">Todas las referencias</option>
            {referencias.map(ref => (
              <option key={ref.id} value={ref.id}>{ref.nombre}</option>
            ))}
          </select>
          <span style={{ marginLeft: '20px', fontSize: '16px', fontWeight: 'bold' }}>
            Total disponible: {totalDisponible} unidades
          </span>
        </div>
      </div>

      <div className="card">
        <table className="table">
          <thead>
            <tr>
              <th>Referencia</th>
              <th>Talla</th>
              <th>Color</th>
              <th>Lote</th>
              <th>Cantidad Disponible</th>
              <th>Cantidad Reservada</th>
              <th>Fecha Ingreso</th>
              <th>Ubicación</th>
              <th>Estado</th>
            </tr>
          </thead>
          <tbody>
            {inventarioFiltrado.map(item => (
              <tr key={item.id}>
                <td><strong>{item.referenciaNombre}</strong></td>
                <td>{item.tallaNombre}</td>
                <td>
                  <span style={{ 
                    display: 'inline-block', 
                    padding: '2px 8px', 
                    borderRadius: '4px', 
                    backgroundColor: '#f0f0f0' 
                  }}>
                    {item.colorNombre}
                  </span>
                </td>
                <td>{item.codigoLote || '-'}</td>
                <td>
                  <strong style={{ color: item.cantidadDisponible > 0 ? '#28a745' : '#dc3545' }}>
                    {item.cantidadDisponible}
                  </strong>
                </td>
                <td>{item.cantidadReservada}</td>
                <td>{item.fechaIngreso ? new Date(item.fechaIngreso).toLocaleDateString() : '-'}</td>
                <td>{item.ubicacion || '-'}</td>
                <td>
                  <span className={`badge ${item.estadoInventario === 'Disponible' ? 'badge-success' : 'badge-secondary'}`}>
                    {item.estadoInventario}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className="card" style={{ marginTop: '20px', padding: '20px' }}>
        <h4>Resumen de Inventario</h4>
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4, 1fr)', gap: '20px', marginTop: '15px' }}>
          <div className="stat-card">
            <h5>Total Items</h5>
            <p className="stat-value">{inventarioFiltrado.length}</p>
          </div>
          <div className="stat-card">
            <h5>Total Disponible</h5>
            <p className="stat-value">{totalDisponible}</p>
          </div>
          <div className="stat-card">
            <h5>Total Reservado</h5>
            <p className="stat-value">
              {inventarioFiltrado.reduce((sum, item) => sum + item.cantidadReservada, 0)}
            </p>
          </div>
          <div className="stat-card">
            <h5>Referencias</h5>
            <p className="stat-value">
              {new Set(inventarioFiltrado.map(item => item.referenciaId)).size}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Inventario;
