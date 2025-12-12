import React, { useState, useEffect } from 'react';
import { avancesApi, asignacionesApi } from '../services/api';

function Archivos() {
  const [avances, setAvances] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filtroAsignacion, setFiltroAsignacion] = useState('');
  const [asignaciones, setAsignaciones] = useState([]);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [asignacionesRes] = await Promise.all([
        asignacionesApi.getAll()
      ]);
      setAsignaciones(asignacionesRes.data || []);
      
      // Cargar todos los avances con fotos
      const avancesConFotos = [];
      for (const asig of asignacionesRes.data || []) {
        try {
          const avancesRes = await avancesApi.getByAsignacion(asig.id);
          const avancesConFoto = (avancesRes.data || []).filter(av => av.urlFotoEvidencia);
          avancesConFotos.push(...avancesConFoto.map(av => ({
            ...av,
            asignacionCodigo: asig.codigoAsignacion,
            tallerNombre: asig.tallerNombre,
            referenciaNombre: asig.referenciaNombre
          })));
        } catch (err) {
          console.error(`Error cargando avances de asignación ${asig.id}:`, err);
        }
      }
      setAvances(avancesConFotos);
    } catch (error) {
      console.error('Error cargando datos:', error);
    } finally {
      setLoading(false);
    }
  };

  const getFullImageUrl = (url) => {
    if (!url) return '';
    if (url.startsWith('http')) return url;
    return `https://localhost:5001${url}`;
  };

  const descargarImagen = (url, nombre) => {
    const link = document.createElement('a');
    link.href = getFullImageUrl(url);
    link.download = nombre || 'evidencia.jpg';
    link.target = '_blank';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  const avancesFiltrados = filtroAsignacion 
    ? avances.filter(av => av.asignacionTallerId === parseInt(filtroAsignacion))
    : avances;

  if (loading) return <div>Cargando archivos...</div>;

  return (
    <div>
      <h2>📁 Galería de Archivos</h2>

      <div className="card" style={{ marginBottom: '20px', padding: '15px' }}>
        <div style={{ display: 'flex', gap: '15px', alignItems: 'center' }}>
          <label style={{ marginBottom: 0 }}>Filtrar por Asignación:</label>
          <select 
            className="form-control" 
            style={{ width: 'auto', flex: 1 }}
            value={filtroAsignacion}
            onChange={(e) => setFiltroAsignacion(e.target.value)}
          >
            <option value="">Todas las asignaciones</option>
            {asignaciones.map(asig => (
              <option key={asig.id} value={asig.id}>
                {asig.codigoAsignacion} - {asig.tallerNombre} - {asig.referenciaNombre}
              </option>
            ))}
          </select>
          <button 
            className="btn btn-secondary"
            onClick={() => setFiltroAsignacion('')}
          >
            Limpiar Filtro
          </button>
        </div>
      </div>

      {avancesFiltrados.length === 0 ? (
        <div className="card" style={{ padding: '20px' }}>
          <div className="alert alert-info">
            No hay archivos de evidencia disponibles. Las fotos se suben al registrar avances de producción.
          </div>
        </div>
      ) : (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: '20px' }}>
          {avancesFiltrados.map((avance) => (
            <div key={avance.id} className="card" style={{ overflow: 'hidden' }}>
              <div style={{ 
                width: '100%', 
                height: '200px', 
                overflow: 'hidden', 
                backgroundColor: '#f5f5f5',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center'
              }}>
                <img 
                  src={getFullImageUrl(avance.urlFotoEvidencia)} 
                  alt="Evidencia"
                  style={{ 
                    maxWidth: '100%', 
                    maxHeight: '100%',
                    objectFit: 'contain'
                  }}
                  onError={(e) => {
                    e.target.style.display = 'none';
                    e.target.parentElement.innerHTML = '<div style="padding: 20px; text-align: center;">❌ Error al cargar imagen</div>';
                  }}
                />
              </div>
              <div style={{ padding: '15px' }}>
                <h5 style={{ marginBottom: '10px', fontSize: '16px' }}>
                  {avance.asignacionCodigo}
                </h5>
                <p style={{ marginBottom: '5px', fontSize: '14px', color: '#666' }}>
                  <strong>Taller:</strong> {avance.tallerNombre}
                </p>
                <p style={{ marginBottom: '5px', fontSize: '14px', color: '#666' }}>
                  <strong>Referencia:</strong> {avance.referenciaNombre}
                </p>
                <p style={{ marginBottom: '5px', fontSize: '14px', color: '#666' }}>
                  <strong>Fecha:</strong> {new Date(avance.fechaRegistro).toLocaleDateString()}
                </p>
                <p style={{ marginBottom: '10px', fontSize: '14px', color: '#666' }}>
                  <strong>Avance:</strong> {avance.porcentajeAvance?.toFixed(1) || 0}%
                </p>
                {avance.observaciones && (
                  <p style={{ marginBottom: '10px', fontSize: '13px', fontStyle: 'italic', color: '#888' }}>
                    "{avance.observaciones}"
                  </p>
                )}
                <div style={{ display: 'flex', gap: '10px' }}>
                  <button 
                    className="btn btn-sm btn-primary"
                    onClick={() => window.open(getFullImageUrl(avance.urlFotoEvidencia), '_blank')}
                    style={{ flex: 1 }}
                  >
                    Ver
                  </button>
                  <button 
                    className="btn btn-sm btn-success"
                    onClick={() => descargarImagen(avance.urlFotoEvidencia, `evidencia-${avance.asignacionCodigo}.jpg`)}
                    style={{ flex: 1 }}
                  >
                    Descargar
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      <div className="card" style={{ marginTop: '20px', padding: '15px', backgroundColor: '#f8f9fa' }}>
        <h5 style={{ marginBottom: '10px' }}>📊 Estadísticas</h5>
        <p style={{ marginBottom: '5px' }}>
          <strong>Total de archivos:</strong> {avancesFiltrados.length}
        </p>
        <p style={{ marginBottom: '5px' }}>
          <strong>Asignaciones con evidencia:</strong> {new Set(avancesFiltrados.map(av => av.asignacionTallerId)).size}
        </p>
        <p style={{ marginBottom: 0 }}>
          <strong>Filtro activo:</strong> {filtroAsignacion ? 'Sí' : 'No'}
        </p>
      </div>
    </div>
  );
}

export default Archivos;
