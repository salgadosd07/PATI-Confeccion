import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { asignacionesApi } from '../services/api';

function Asignaciones() {
  const [asignaciones, setAsignaciones] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadAsignaciones();
  }, []);

  const loadAsignaciones = async () => {
    try {
      const response = await asignacionesApi.getAll();
      setAsignaciones(response.data);
    } catch (error) {
      console.error('Error cargando asignaciones:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div>Cargando...</div>;

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <h2>Asignaciones a Talleres</h2>
        <Link to="/asignaciones/crear" className="btn btn-primary">+ Nueva Asignación</Link>
      </div>

      <div className="card">
        <table className="table">
          <thead>
            <tr>
              <th>Código</th>
              <th>Taller</th>
              <th>Referencia</th>
              <th>Lote</th>
              <th>Fecha Asignación</th>
              <th>Fecha Estimada</th>
              <th>Cantidad</th>
              <th>Avance</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {asignaciones.map(asig => (
              <tr key={asig.id}>
                <td>{asig.codigoAsignacion}</td>
                <td>{asig.tallerNombre}</td>
                <td>{asig.referenciaNombre}</td>
                <td>{asig.codigoLote}</td>
                <td>{new Date(asig.fechaAsignacion).toLocaleDateString()}</td>
                <td>{asig.fechaEstimadaEntrega ? new Date(asig.fechaEstimadaEntrega).toLocaleDateString() : '-'}</td>
                <td>{asig.cantidadAsignada}</td>
                <td>{asig.porcentajeAvance ? `${asig.porcentajeAvance.toFixed(1)}%` : '0%'}</td>
                <td>
                  <Link to={`/asignaciones/${asig.id}/avance`} className="btn btn-sm btn-success">
                    Registrar Avance
                  </Link>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default Asignaciones;
