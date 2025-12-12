import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { remisionesApi, asignacionesApi, tallasApi, coloresApi } from '../services/api';

function CrearRemision() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [asignaciones, setAsignaciones] = useState([]);
  const [tallas, setTallas] = useState([]);
  const [colores, setColores] = useState([]);
  const [formData, setFormData] = useState({
    asignacionTallerId: '',
    fechaDespacho: new Date().toISOString().split('T')[0],
    cantidadEnviada: 0,
    observaciones: '',
    detalles: []
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [asigResponse, tallasResponse, coloresResponse] = await Promise.all([
        asignacionesApi.getAll(),
        tallasApi.getAll(),
        coloresApi.getAll()
      ]);
      setAsignaciones(asigResponse.data);
      setTallas(tallasResponse.data);
      setColores(coloresResponse.data);
    } catch (error) {
      console.error('Error cargando datos:', error);
      alert('Error al cargar los datos');
    }
  };

  const agregarDetalle = () => {
    setFormData({
      ...formData,
      detalles: [...formData.detalles, { tallaId: '', colorId: '', cantidad: 0 }]
    });
  };

  const actualizarDetalle = (index, field, value) => {
    const nuevosDetalles = [...formData.detalles];
    nuevosDetalles[index][field] = field === 'cantidad' ? parseInt(value) || 0 : value;
    
    // Recalcular cantidad total
    const cantidadTotal = nuevosDetalles.reduce((sum, det) => sum + det.cantidad, 0);
    
    setFormData({
      ...formData,
      detalles: nuevosDetalles,
      cantidadEnviada: cantidadTotal
    });
  };

  const eliminarDetalle = (index) => {
    const nuevosDetalles = formData.detalles.filter((_, i) => i !== index);
    const cantidadTotal = nuevosDetalles.reduce((sum, det) => sum + det.cantidad, 0);
    
    setFormData({
      ...formData,
      detalles: nuevosDetalles,
      cantidadEnviada: cantidadTotal
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!formData.asignacionTallerId) {
      alert('Seleccione una asignación');
      return;
    }

    if (formData.detalles.length === 0) {
      alert('Agregue al menos un detalle');
      return;
    }

    // Validar detalles
    for (const detalle of formData.detalles) {
      if (!detalle.tallaId || !detalle.colorId || detalle.cantidad <= 0) {
        alert('Complete todos los campos de los detalles');
        return;
      }
    }

    setLoading(true);
    try {
      await remisionesApi.create({
        asignacionTallerId: parseInt(formData.asignacionTallerId),
        fechaDespacho: new Date(formData.fechaDespacho).toISOString(),
        cantidadEnviada: formData.cantidadEnviada,
        observaciones: formData.observaciones,
        detalles: formData.detalles.map(d => ({
          tallaId: parseInt(d.tallaId),
          colorId: parseInt(d.colorId),
          cantidad: d.cantidad
        }))
      });
      alert('Remisión creada exitosamente');
      navigate('/remisiones');
    } catch (error) {
      console.error('Error creando remisión:', error);
      alert('Error al crear la remisión: ' + (error.response?.data?.message || error.message));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h2>Crear Nueva Remisión</h2>
      
      <div className="card" style={{ padding: '20px' }}>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Asignación al Taller *</label>
            <select
              className="form-control"
              value={formData.asignacionTallerId}
              onChange={(e) => setFormData({ ...formData, asignacionTallerId: e.target.value })}
              required
            >
              <option value="">Seleccione una asignación...</option>
              {asignaciones.map(asig => (
                <option key={asig.id} value={asig.id}>
                  {asig.codigoAsignacion} - {asig.tallerNombre} - {asig.referenciaNombre}
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Fecha de Despacho *</label>
            <input
              type="date"
              className="form-control"
              value={formData.fechaDespacho}
              onChange={(e) => setFormData({ ...formData, fechaDespacho: e.target.value })}
              required
            />
          </div>

          <div className="form-group">
            <label>Cantidad Total Enviada</label>
            <input
              type="number"
              className="form-control"
              value={formData.cantidadEnviada}
              readOnly
              style={{ backgroundColor: '#f0f0f0' }}
            />
            <small className="form-text text-muted">Se calcula automáticamente desde los detalles</small>
          </div>

          <div className="form-group">
            <label>Observaciones</label>
            <textarea
              className="form-control"
              rows="3"
              value={formData.observaciones}
              onChange={(e) => setFormData({ ...formData, observaciones: e.target.value })}
            />
          </div>

          <hr />

          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
            <h4>Detalles de la Remisión</h4>
            <button type="button" onClick={agregarDetalle} className="btn btn-success">
              + Agregar Detalle
            </button>
          </div>

          {formData.detalles.map((detalle, index) => (
            <div key={index} className="card" style={{ padding: '15px', marginBottom: '10px', backgroundColor: '#f9f9f9' }}>
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr auto', gap: '10px', alignItems: 'end' }}>
                <div className="form-group">
                  <label>Talla</label>
                  <select
                    className="form-control"
                    value={detalle.tallaId}
                    onChange={(e) => actualizarDetalle(index, 'tallaId', e.target.value)}
                    required
                  >
                    <option value="">Seleccione...</option>
                    {tallas.map(talla => (
                      <option key={talla.id} value={talla.id}>{talla.nombre}</option>
                    ))}
                  </select>
                </div>

                <div className="form-group">
                  <label>Color</label>
                  <select
                    className="form-control"
                    value={detalle.colorId}
                    onChange={(e) => actualizarDetalle(index, 'colorId', e.target.value)}
                    required
                  >
                    <option value="">Seleccione...</option>
                    {colores.map(color => (
                      <option key={color.id} value={color.id}>{color.nombre}</option>
                    ))}
                  </select>
                </div>

                <div className="form-group">
                  <label>Cantidad</label>
                  <input
                    type="number"
                    className="form-control"
                    value={detalle.cantidad}
                    onChange={(e) => actualizarDetalle(index, 'cantidad', e.target.value)}
                    min="1"
                    required
                  />
                </div>

                <button
                  type="button"
                  onClick={() => eliminarDetalle(index)}
                  className="btn btn-danger"
                  style={{ height: '38px' }}
                >
                  ✕
                </button>
              </div>
            </div>
          ))}

          {formData.detalles.length === 0 && (
            <div style={{ padding: '20px', textAlign: 'center', color: '#999' }}>
              No hay detalles agregados. Haga clic en "Agregar Detalle" para comenzar.
            </div>
          )}

          <div style={{ display: 'flex', gap: '10px', marginTop: '20px' }}>
            <button type="submit" disabled={loading} className="btn btn-primary">
              {loading ? 'Guardando...' : 'Crear Remisión'}
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

export default CrearRemision;
