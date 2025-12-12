import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { remisionesApi } from '../services/api';

function Remisiones() {
  const [remisiones, setRemisiones] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadRemisiones();
  }, []);

  const loadRemisiones = async () => {
    try {
      const response = await remisionesApi.getAll();
      setRemisiones(response.data);
    } catch (error) {
      console.error('Error cargando remisiones:', error);
    } finally {
      setLoading(false);
    }
  };

  const getEstadoBadge = (estado) => {
    const badges = {
      'Despachada': 'badge-warning',
      'Recibida': 'badge-success',
      'Pendiente': 'badge-secondary',
    };
    return badges[estado] || 'badge-secondary';
  };

  if (loading) return <div>Cargando...</div>;

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <h2>Remisiones</h2>
        <div>
          <Link to="/remisiones/crear" className="btn btn-primary" style={{ marginRight: '10px' }}>
            Crear Remisión
          </Link>
          <Link to="/control-calidad/crear" className="btn btn-success">
            Registrar Control Calidad
          </Link>
        </div>
      </div>

      <div className="card">
        <table className="table">
          <thead>
            <tr>
              <th>Número Remisión</th>
              <th>Taller</th>
              <th>Referencia</th>
              <th>Fecha Despacho</th>
              <th>Fecha Recepción</th>
              <th>Cant. Enviada</th>
              <th>Cant. Recibida</th>
              <th>Estado</th>
              <th>Revisado Por</th>
            </tr>
          </thead>
          <tbody>
            {remisiones.map(remision => (
              <tr key={remision.id}>
                <td><strong>{remision.numeroRemision}</strong></td>
                <td>{remision.tallerNombre}</td>
                <td>{remision.referenciaNombre}</td>
                <td>{new Date(remision.fechaDespacho).toLocaleDateString()}</td>
                <td>{remision.fechaRecepcion ? new Date(remision.fechaRecepcion).toLocaleDateString() : '-'}</td>
                <td>{remision.cantidadEnviada}</td>
                <td>{remision.cantidadRecibida || '-'}</td>
                <td>
                  <span className={`badge ${getEstadoBadge(remision.estadoRemision)}`}>
                    {remision.estadoRemision}
                  </span>
                </td>
                <td>{remision.revisadoPor || '-'}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default Remisiones;
