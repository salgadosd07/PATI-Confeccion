import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { asignacionesApi, talleresApi, referenciasApi, cortesApi } from '../services/api';

function CrearAsignacion() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    tallerId: '',
    referenciaId: '',
    corteId: '',
    fechaAsignacion: new Date().toISOString().split('T')[0],
    fechaEstimadaEntrega: '',
    cantidadAsignada: 0,
    valorUnitario: 0,
    observaciones: ''
  });

  const [talleres, setTalleres] = useState([]);
  const [referencias, setReferencias] = useState([]);
  const [cortes, setCortes] = useState([]);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [talResp, refResp, corResp] = await Promise.all([
        talleresApi.getAll(),
        referenciasApi.getAll(),
        cortesApi.getAll()
      ]);
      setTalleres(talResp.data);
      setReferencias(refResp.data);
      setCortes(corResp.data);
    } catch (error) {
      console.error('Error cargando datos:', error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await asignacionesApi.create(formData);
      navigate('/asignaciones');
    } catch (error) {
      console.error('Error creando asignación:', error);
      alert('Error al crear la asignación');
    }
  };

  return (
    <div>
      <h2>Crear Nueva Asignación</h2>
      <div className="card">
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Taller</label>
            <select
              value={formData.tallerId}
              onChange={(e) => setFormData({ ...formData, tallerId: parseInt(e.target.value) })}
              required
            >
              <option value="">Seleccione un taller</option>
              {talleres.map(taller => (
                <option key={taller.id} value={taller.id}>{taller.nombre}</option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Referencia</label>
            <select
              value={formData.referenciaId}
              onChange={(e) => setFormData({ ...formData, referenciaId: parseInt(e.target.value) })}
              required
            >
              <option value="">Seleccione una referencia</option>
              {referencias.map(ref => (
                <option key={ref.id} value={ref.id}>{ref.nombre}</option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Corte</label>
            <select
              value={formData.corteId}
              onChange={(e) => setFormData({ ...formData, corteId: parseInt(e.target.value) })}
              required
            >
              <option value="">Seleccione un corte</option>
              {cortes.map(corte => (
                <option key={corte.id} value={corte.id}>{corte.codigoLote} - {corte.referenciaNombre}</option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Fecha de Asignación</label>
            <input
              type="date"
              value={formData.fechaAsignacion}
              onChange={(e) => setFormData({ ...formData, fechaAsignacion: e.target.value })}
              required
            />
          </div>

          <div className="form-group">
            <label>Fecha Estimada de Entrega</label>
            <input
              type="date"
              value={formData.fechaEstimadaEntrega}
              onChange={(e) => setFormData({ ...formData, fechaEstimadaEntrega: e.target.value })}
            />
          </div>

          <div className="form-group">
            <label>Cantidad Asignada</label>
            <input
              type="number"
              value={formData.cantidadAsignada}
              onChange={(e) => setFormData({ ...formData, cantidadAsignada: parseInt(e.target.value) })}
              required
            />
          </div>

          <div className="form-group">
            <label>Valor Unitario</label>
            <input
              type="number"
              step="0.01"
              value={formData.valorUnitario}
              onChange={(e) => setFormData({ ...formData, valorUnitario: parseFloat(e.target.value) })}
            />
          </div>

          <div className="form-group">
            <label>Observaciones</label>
            <textarea
              value={formData.observaciones}
              onChange={(e) => setFormData({ ...formData, observaciones: e.target.value })}
              rows="3"
            />
          </div>

          <div style={{ display: 'flex', gap: '10px' }}>
            <button type="submit" className="btn btn-success">Guardar Asignación</button>
            <button type="button" onClick={() => navigate('/asignaciones')} className="btn">Cancelar</button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default CrearAsignacion;
