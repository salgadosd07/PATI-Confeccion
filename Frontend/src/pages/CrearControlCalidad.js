import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { controlCalidadApi, remisionesApi } from '../services/api';

function CrearControlCalidad() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [loadingRemisiones, setLoadingRemisiones] = useState(true);
  const [remisiones, setRemisiones] = useState([]);
  const [error, setError] = useState(null);
  const [formData, setFormData] = useState({
    remisionId: '',
    cantidadImperfectos: 0,
    cantidadArreglos: 0,
    cantidadPendientes: 0,
    cantidadAprobados: 0,
    causaImperfecto: '',
    observaciones: '',
    revisadoPor: '',
    detallesImperfectos: []
  });

  useEffect(() => {
    loadRemisiones();
  }, []);

  const loadRemisiones = async () => {
    setLoadingRemisiones(true);
    setError(null);
    try {
      const response = await remisionesApi.getAll();
      console.log('Todas las remisiones:', response.data);
      
      // Filtrar solo remisiones recibidas
      const remisionesRecibidas = (response.data || []).filter(r => r.estadoRemision === 'Recibida');
      console.log('Remisiones recibidas:', remisionesRecibidas);
      
      setRemisiones(remisionesRecibidas);
      
      if (remisionesRecibidas.length === 0) {
        setError('No hay remisiones en estado "Recibida" disponibles para inspección. Primero debe crear y recibir una remisión.');
      }
    } catch (error) {
      console.error('Error cargando remisiones:', error);
      setError('Error al cargar las remisiones: ' + (error.response?.data?.message || error.message));
    } finally {
      setLoadingRemisiones(false);
    }
  };

  const agregarDetalleImperfecto = () => {
    setFormData({
      ...formData,
      detallesImperfectos: [...formData.detallesImperfectos, { tipoDefecto: '', descripcion: '', cantidad: 0 }]
    });
  };

  const actualizarDetalleImperfecto = (index, field, value) => {
    const nuevosDetalles = [...formData.detallesImperfectos];
    nuevosDetalles[index][field] = field === 'cantidad' ? parseInt(value) || 0 : value;
    setFormData({
      ...formData,
      detallesImperfectos: nuevosDetalles
    });
  };

  const eliminarDetalleImperfecto = (index) => {
    const nuevosDetalles = formData.detallesImperfectos.filter((_, i) => i !== index);
    setFormData({
      ...formData,
      detallesImperfectos: nuevosDetalles
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!formData.remisionId) {
      alert('Seleccione una remisión');
      return;
    }

    const totalVerificado = formData.cantidadImperfectos + 
                           formData.cantidadArreglos + 
                           formData.cantidadPendientes + 
                           formData.cantidadAprobados;

    if (totalVerificado === 0) {
      alert('Ingrese al menos una cantidad en el control de calidad');
      return;
    }

    setLoading(true);
    try {
      await controlCalidadApi.create({
        remisionId: parseInt(formData.remisionId),
        cantidadImperfectos: formData.cantidadImperfectos,
        cantidadArreglos: formData.cantidadArreglos,
        cantidadPendientes: formData.cantidadPendientes,
        cantidadAprobados: formData.cantidadAprobados,
        causaImperfecto: formData.causaImperfecto,
        observaciones: formData.observaciones,
        revisadoPor: formData.revisadoPor,
        detallesImperfectos: formData.detallesImperfectos
      });
      alert('Control de calidad registrado exitosamente');
      navigate('/remisiones');
    } catch (error) {
      console.error('Error creando control de calidad:', error);
      alert('Error al registrar el control de calidad: ' + (error.response?.data?.message || error.message));
    } finally {
      setLoading(false);
    }
  };

  if (loadingRemisiones) {
    return <div>Cargando remisiones disponibles...</div>;
  }

  return (
    <div>
      <h2>Registrar Control de Calidad</h2>
      
      {error && (
        <div className="alert alert-warning" style={{ marginBottom: '20px' }}>
          {error}
        </div>
      )}
      
      <div className="card" style={{ padding: '20px' }}>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Remisión Recibida *</label>
            <select
              className="form-control"
              value={formData.remisionId}
              onChange={(e) => setFormData({ ...formData, remisionId: e.target.value })}
              required
              disabled={remisiones.length === 0}
            >
              <option value="">
                {remisiones.length === 0 
                  ? 'No hay remisiones disponibles' 
                  : 'Seleccione una remisión...'}
              </option>
              {remisiones.map(rem => (
                <option key={rem.id} value={rem.id}>
                  {rem.numeroRemision} - {rem.tallerNombre || 'Sin taller'} - Cantidad: {rem.cantidadRecibida || rem.cantidadEnviada}
                </option>
              ))}
            </select>
            {remisiones.length === 0 && (
              <small className="form-text text-muted">
                Debe tener al menos una remisión en estado "Recibida" para poder registrar control de calidad.
              </small>
            )}
          </div>

          <h4 style={{ marginTop: '20px', marginBottom: '15px' }}>Resultados de Inspección</h4>

          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(2, 1fr)', gap: '15px' }}>
            <div className="form-group">
              <label>Cantidad Aprobados</label>
              <input
                type="number"
                className="form-control"
                value={formData.cantidadAprobados}
                onChange={(e) => setFormData({ ...formData, cantidadAprobados: parseInt(e.target.value) || 0 })}
                min="0"
              />
            </div>

            <div className="form-group">
              <label>Cantidad Imperfectos</label>
              <input
                type="number"
                className="form-control"
                value={formData.cantidadImperfectos}
                onChange={(e) => setFormData({ ...formData, cantidadImperfectos: parseInt(e.target.value) || 0 })}
                min="0"
              />
            </div>

            <div className="form-group">
              <label>Cantidad Requieren Arreglos</label>
              <input
                type="number"
                className="form-control"
                value={formData.cantidadArreglos}
                onChange={(e) => setFormData({ ...formData, cantidadArreglos: parseInt(e.target.value) || 0 })}
                min="0"
              />
            </div>

            <div className="form-group">
              <label>Cantidad Pendientes</label>
              <input
                type="number"
                className="form-control"
                value={formData.cantidadPendientes}
                onChange={(e) => setFormData({ ...formData, cantidadPendientes: parseInt(e.target.value) || 0 })}
                min="0"
              />
            </div>
          </div>

          <div className="form-group">
            <label>Causa Principal de Imperfectos</label>
            <input
              type="text"
              className="form-control"
              value={formData.causaImperfecto}
              onChange={(e) => setFormData({ ...formData, causaImperfecto: e.target.value })}
              placeholder="Ej: Costuras irregulares, manchas, talla incorrecta..."
            />
          </div>

          <div className="form-group">
            <label>Observaciones Generales</label>
            <textarea
              className="form-control"
              rows="3"
              value={formData.observaciones}
              onChange={(e) => setFormData({ ...formData, observaciones: e.target.value })}
            />
          </div>

          <div className="form-group">
            <label>Revisado Por *</label>
            <input
              type="text"
              className="form-control"
              value={formData.revisadoPor}
              onChange={(e) => setFormData({ ...formData, revisadoPor: e.target.value })}
              placeholder="Nombre del inspector de calidad"
              required
            />
          </div>

          <hr />

          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
            <h4>Detalles de Imperfectos</h4>
            <button type="button" onClick={agregarDetalleImperfecto} className="btn btn-success">
              + Agregar Detalle
            </button>
          </div>

          {formData.detallesImperfectos.map((detalle, index) => (
            <div key={index} className="card" style={{ padding: '15px', marginBottom: '10px', backgroundColor: '#fff3cd' }}>
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 2fr 100px auto', gap: '10px', alignItems: 'end' }}>
                <div className="form-group">
                  <label>Tipo de Defecto</label>
                  <select
                    className="form-control"
                    value={detalle.tipoDefecto}
                    onChange={(e) => actualizarDetalleImperfecto(index, 'tipoDefecto', e.target.value)}
                  >
                    <option value="">Seleccione...</option>
                    <option value="Costura">Costura</option>
                    <option value="Mancha">Mancha</option>
                    <option value="Talla">Talla Incorrecta</option>
                    <option value="Color">Color</option>
                    <option value="Corte">Corte/Patrón</option>
                    <option value="Otro">Otro</option>
                  </select>
                </div>

                <div className="form-group">
                  <label>Descripción</label>
                  <input
                    type="text"
                    className="form-control"
                    value={detalle.descripcion}
                    onChange={(e) => actualizarDetalleImperfecto(index, 'descripcion', e.target.value)}
                    placeholder="Describa el defecto..."
                  />
                </div>

                <div className="form-group">
                  <label>Cantidad</label>
                  <input
                    type="number"
                    className="form-control"
                    value={detalle.cantidad}
                    onChange={(e) => actualizarDetalleImperfecto(index, 'cantidad', e.target.value)}
                    min="1"
                  />
                </div>

                <button
                  type="button"
                  onClick={() => eliminarDetalleImperfecto(index)}
                  className="btn btn-danger"
                  style={{ height: '38px' }}
                >
                  ✕
                </button>
              </div>
            </div>
          ))}

          <div style={{ display: 'flex', gap: '10px', marginTop: '20px' }}>
            <button type="submit" disabled={loading} className="btn btn-primary">
              {loading ? 'Guardando...' : 'Registrar Control de Calidad'}
            </button>
            <button type="button" onClick={() => navigate('/remisiones')} className="btn btn-secondary">
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default CrearControlCalidad;
