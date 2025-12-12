import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { avancesApi, asignacionesApi, archivosApi } from '../services/api';

function RegistrarAvance() {
  const navigate = useNavigate();
  const { asignacionId } = useParams();
  const [loading, setLoading] = useState(false);
  const [uploadingFile, setUploadingFile] = useState(false);
  const [asignaciones, setAsignaciones] = useState([]);
  const [asignacionSeleccionada, setAsignacionSeleccionada] = useState(null);
  const [formData, setFormData] = useState({
    asignacionTallerId: asignacionId || '',
    cantidadLista: 0,
    cantidadEnProceso: 0,
    cantidadPendiente: 0,
    cantidadDespachada: 0,
    observaciones: '',
    urlFotoEvidencia: ''
  });

  useEffect(() => {
    loadAsignaciones();
  }, []);

  useEffect(() => {
    if (asignacionId && asignaciones.length > 0) {
      const asignacion = asignaciones.find(a => a.id === parseInt(asignacionId));
      if (asignacion) {
        setAsignacionSeleccionada(asignacion);
        setFormData(prev => ({
          ...prev,
          cantidadPendiente: asignacion.cantidadAsignada
        }));
      }
    }
  }, [asignacionId, asignaciones]);

  const loadAsignaciones = async () => {
    try {
      const response = await asignacionesApi.getAll();
      setAsignaciones(response.data);
    } catch (error) {
      console.error('Error cargando asignaciones:', error);
      alert('Error al cargar las asignaciones');
    }
  };

  const handleAsignacionChange = (asignacionId) => {
    const asignacion = asignaciones.find(a => a.id === parseInt(asignacionId));
    setAsignacionSeleccionada(asignacion);
    setFormData({
      ...formData,
      asignacionTallerId: asignacionId,
      cantidadPendiente: asignacion?.cantidadAsignada || 0
    });
  };

  const handleFileUpload = async (e) => {
    const file = e.target.files[0];
    if (!file) return;

    // Validar tipo de archivo
    if (!file.type.startsWith('image/')) {
      alert('Solo se permiten archivos de imagen');
      return;
    }

    // Validar tamaño (máx 5MB)
    if (file.size > 5 * 1024 * 1024) {
      alert('El archivo no debe superar 5MB');
      return;
    }

    setUploadingFile(true);
    try {
      const formDataFile = new FormData();
      formDataFile.append('file', file);

      const response = await archivosApi.upload(formDataFile);
      setFormData({
        ...formData,
        urlFotoEvidencia: response.data.url
      });
      alert('Imagen subida exitosamente');
    } catch (error) {
      console.error('Error subiendo archivo:', error);
      alert('Error al subir la imagen');
    } finally {
      setUploadingFile(false);
    }
  };

  const calcularPorcentaje = () => {
    if (!asignacionSeleccionada) return 0;
    return ((formData.cantidadLista / asignacionSeleccionada.cantidadAsignada) * 100).toFixed(1);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!formData.asignacionTallerId) {
      alert('Seleccione una asignación');
      return;
    }

    const totalReportado = formData.cantidadLista + 
                          formData.cantidadEnProceso + 
                          formData.cantidadPendiente;

    if (totalReportado === 0) {
      alert('Debe reportar al menos una cantidad');
      return;
    }

    setLoading(true);
    try {
      await avancesApi.create({
        asignacionTallerId: parseInt(formData.asignacionTallerId),
        cantidadLista: formData.cantidadLista,
        cantidadEnProceso: formData.cantidadEnProceso,
        cantidadPendiente: formData.cantidadPendiente,
        cantidadDespachada: formData.cantidadDespachada,
        observaciones: formData.observaciones,
        urlFotoEvidencia: formData.urlFotoEvidencia
      });
      alert('Avance registrado exitosamente');
      navigate('/asignaciones');
    } catch (error) {
      console.error('Error registrando avance:', error);
      alert('Error al registrar el avance: ' + (error.response?.data?.message || error.message));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h2>Registrar Avance de Producción</h2>
      
      <div className="card" style={{ padding: '20px' }}>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Asignación *</label>
            <select
              className="form-control"
              value={formData.asignacionTallerId}
              onChange={(e) => handleAsignacionChange(e.target.value)}
              required
              disabled={!!asignacionId}
            >
              <option value="">Seleccione una asignación...</option>
              {asignaciones.map(asig => (
                <option key={asig.id} value={asig.id}>
                  {asig.codigoAsignacion} - {asig.tallerNombre} - {asig.referenciaNombre}
                </option>
              ))}
            </select>
          </div>

          {asignacionSeleccionada && (
            <div className="alert alert-info">
              <strong>Información de la Asignación:</strong>
              <ul style={{ marginBottom: 0, marginTop: '10px' }}>
                <li>Cantidad Asignada: {asignacionSeleccionada.cantidadAsignada} unidades</li>
                <li>Avance Actual: {asignacionSeleccionada.porcentajeAvance?.toFixed(1) || 0}%</li>
                {asignacionSeleccionada.fechaEstimadaEntrega && (
                  <li>Fecha Estimada: {new Date(asignacionSeleccionada.fechaEstimadaEntrega).toLocaleDateString()}</li>
                )}
              </ul>
            </div>
          )}

          <h4 style={{ marginTop: '20px', marginBottom: '15px' }}>Estado de Producción</h4>

          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(2, 1fr)', gap: '15px' }}>
            <div className="form-group">
              <label>Cantidad Lista ✓</label>
              <input
                type="number"
                className="form-control"
                value={formData.cantidadLista}
                onChange={(e) => setFormData({ ...formData, cantidadLista: parseInt(e.target.value) || 0 })}
                min="0"
              />
              <small className="form-text text-muted">Prendas terminadas y listas para enviar</small>
            </div>

            <div className="form-group">
              <label>Cantidad en Proceso 🔄</label>
              <input
                type="number"
                className="form-control"
                value={formData.cantidadEnProceso}
                onChange={(e) => setFormData({ ...formData, cantidadEnProceso: parseInt(e.target.value) || 0 })}
                min="0"
              />
              <small className="form-text text-muted">Prendas en proceso de confección</small>
            </div>

            <div className="form-group">
              <label>Cantidad Pendiente ⏳</label>
              <input
                type="number"
                className="form-control"
                value={formData.cantidadPendiente}
                onChange={(e) => setFormData({ ...formData, cantidadPendiente: parseInt(e.target.value) || 0 })}
                min="0"
              />
              <small className="form-text text-muted">Prendas sin iniciar</small>
            </div>

            <div className="form-group">
              <label>Cantidad Despachada 📦</label>
              <input
                type="number"
                className="form-control"
                value={formData.cantidadDespachada}
                onChange={(e) => setFormData({ ...formData, cantidadDespachada: parseInt(e.target.value) || 0 })}
                min="0"
              />
              <small className="form-text text-muted">Prendas ya enviadas a la empresa</small>
            </div>
          </div>

          {asignacionSeleccionada && (
            <div className="alert alert-success" style={{ marginTop: '15px' }}>
              <strong>Porcentaje de Avance: {calcularPorcentaje()}%</strong>
              <div style={{ width: '100%', backgroundColor: '#e0e0e0', borderRadius: '5px', marginTop: '10px', height: '20px' }}>
                <div style={{ 
                  width: `${calcularPorcentaje()}%`, 
                  backgroundColor: '#28a745', 
                  height: '100%', 
                  borderRadius: '5px',
                  transition: 'width 0.3s'
                }}></div>
              </div>
            </div>
          )}

          <div className="form-group">
            <label>Foto de Evidencia</label>
            <input
              type="file"
              className="form-control"
              accept="image/*"
              onChange={handleFileUpload}
              disabled={uploadingFile}
            />
            {uploadingFile && <small className="form-text text-muted">Subiendo imagen...</small>}
            {formData.urlFotoEvidencia && (
              <div style={{ marginTop: '10px' }}>
                <small className="text-success">✓ Imagen cargada</small>
                <img 
                  src={formData.urlFotoEvidencia} 
                  alt="Evidencia" 
                  style={{ maxWidth: '200px', marginTop: '10px', display: 'block', borderRadius: '5px' }}
                />
              </div>
            )}
          </div>

          <div className="form-group">
            <label>Observaciones</label>
            <textarea
              className="form-control"
              rows="3"
              value={formData.observaciones}
              onChange={(e) => setFormData({ ...formData, observaciones: e.target.value })}
              placeholder="Novedades, dificultades, comentarios..."
            />
          </div>

          <div style={{ display: 'flex', gap: '10px', marginTop: '20px' }}>
            <button type="submit" disabled={loading} className="btn btn-primary">
              {loading ? 'Guardando...' : 'Registrar Avance'}
            </button>
            <button type="button" onClick={() => navigate('/asignaciones')} className="btn btn-secondary">
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default RegistrarAvance;
