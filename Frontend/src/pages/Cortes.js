import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { cortesApi } from '../services/api';

function Cortes() {
  const [cortes, setCortes] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadCortes();
  }, []);

  const loadCortes = async () => {
    try {
      const response = await cortesApi.getAll();
      setCortes(response.data);
    } catch (error) {
      console.error('Error cargando cortes:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div>Cargando...</div>;

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <h2>Gestión de Cortes</h2>
        <Link to="/cortes/crear" className="btn btn-primary">+ Nuevo Corte</Link>
      </div>

      <div className="card">
        <table className="table">
          <thead>
            <tr>
              <th>Código Lote</th>
              <th>Mesa</th>
              <th>Fecha Corte</th>
              <th>Referencia</th>
              <th>Material</th>
              <th>Cantidad Total</th>
              <th>Programada</th>
            </tr>
          </thead>
          <tbody>
            {cortes.map(corte => (
              <tr key={corte.id}>
                <td>{corte.codigoLote}</td>
                <td>{corte.mesa}</td>
                <td>{new Date(corte.fechaCorte).toLocaleDateString()}</td>
                <td>{corte.referenciaNombre}</td>
                <td>{corte.materialNombre}</td>
                <td>{corte.cantidadTotal}</td>
                <td>{corte.cantidadProgramada}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default Cortes;
