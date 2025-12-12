import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { pagosApi, asignacionesApi } from '../services/api';

function CrearPago() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [asignaciones, setAsignaciones] = useState([]);
  const [asignacionSeleccionada, setAsignacionSeleccionada] = useState(null);
  const [formData, setFormData] = useState({
    asignacionTallerId: '',
    montoTotal: 0,
    montoPagado: 0,
    metodoPago: '',
    referencia: '',
    observaciones: ''
  });

  useEffect(() => {
    loadAsignaciones();
  }, []);

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
    
    // Calcular monto basado en valor unitario si existe
    const montoCalculado = asignacion?.valorTotal || 
                          (asignacion?.valorUnitario * asignacion?.cantidadAsignada) || 0;
    
    setFormData({
      ...formData,
      asignacionTallerId: asignacionId,
      montoTotal: montoCalculado
    });
  };

  const calcularPorcentajePago = () => {
    if (formData.montoTotal === 0) return 0;
    return ((formData.montoPagado / formData.montoTotal) * 100).toFixed(1);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!formData.asignacionTallerId) {
      alert('Seleccione una asignación');
      return;
    }

    if (formData.montoTotal <= 0) {
      alert('El monto total debe ser mayor a 0');
      return;
    }

    setLoading(true);
    try {
      await pagosApi.create({
        asignacionTallerId: parseInt(formData.asignacionTallerId),
        montoTotal: parseFloat(formData.montoTotal),
        montoPagado: parseFloat(formData.montoPagado),
        metodoPago: formData.metodoPago,
        referencia: formData.referencia,
        observaciones: formData.observaciones
      });
      alert('Pago registrado exitosamente');
      navigate('/pagos');
    } catch (error) {
      console.error('Error creando pago:', error);
      alert('Error al registrar el pago: ' + (error.response?.data?.message || error.message));
    } finally {
      setLoading(false);
    }
  };

  const formatCurrency = (value) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
    }).format(value);
  };

  return (
    <div>
      <h2>Registrar Pago a Taller</h2>
      
      <div className="card" style={{ padding: '20px' }}>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Asignación al Taller *</label>
            <select
              className="form-control"
              value={formData.asignacionTallerId}
              onChange={(e) => handleAsignacionChange(e.target.value)}
              required
            >
              <option value="">Seleccione una asignación...</option>
              {asignaciones.map(asig => (
                <option key={asig.id} value={asig.id}>
                  {asig.codigoAsignacion} - {asig.tallerNombre} - {asig.referenciaNombre} 
                  ({asig.cantidadAsignada} unidades)
                </option>
              ))}
            </select>
          </div>

          {asignacionSeleccionada && (
            <div className="alert alert-info" style={{ marginTop: '10px' }}>
              <strong>Información de la Asignación:</strong>
              <ul style={{ marginBottom: 0, marginTop: '10px' }}>
                <li>Taller: {asignacionSeleccionada.tallerNombre}</li>
                <li>Referencia: {asignacionSeleccionada.referenciaNombre}</li>
                <li>Cantidad: {asignacionSeleccionada.cantidadAsignada} unidades</li>
                {asignacionSeleccionada.valorUnitario && (
                  <li>Valor Unitario: {formatCurrency(asignacionSeleccionada.valorUnitario)}</li>
                )}
                {asignacionSeleccionada.valorTotal && (
                  <li>Valor Total Asignación: {formatCurrency(asignacionSeleccionada.valorTotal)}</li>
                )}
              </ul>
            </div>
          )}

          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(2, 1fr)', gap: '15px' }}>
            <div className="form-group">
              <label>Monto Total del Pago *</label>
              <input
                type="number"
                step="0.01"
                className="form-control"
                value={formData.montoTotal}
                onChange={(e) => setFormData({ ...formData, montoTotal: parseFloat(e.target.value) || 0 })}
                required
                min="0"
              />
              <small className="form-text text-muted">
                {formatCurrency(formData.montoTotal)}
              </small>
            </div>

            <div className="form-group">
              <label>Monto Pagado</label>
              <input
                type="number"
                step="0.01"
                className="form-control"
                value={formData.montoPagado}
                onChange={(e) => setFormData({ ...formData, montoPagado: parseFloat(e.target.value) || 0 })}
                min="0"
              />
              <small className="form-text text-muted">
                {formatCurrency(formData.montoPagado)} ({calcularPorcentajePago()}%)
              </small>
            </div>
          </div>

          {formData.montoPagado > 0 && formData.montoPagado < formData.montoTotal && (
            <div className="alert alert-warning">
              ⚠️ Este será un pago parcial. Saldo pendiente: {formatCurrency(formData.montoTotal - formData.montoPagado)}
            </div>
          )}

          {formData.montoPagado >= formData.montoTotal && formData.montoTotal > 0 && (
            <div className="alert alert-success">
              ✓ Este pago cubrirá el monto total
            </div>
          )}

          <div className="form-group">
            <label>Método de Pago</label>
            <select
              className="form-control"
              value={formData.metodoPago}
              onChange={(e) => setFormData({ ...formData, metodoPago: e.target.value })}
            >
              <option value="">Seleccione...</option>
              <option value="Efectivo">Efectivo</option>
              <option value="Transferencia">Transferencia Bancaria</option>
              <option value="Cheque">Cheque</option>
              <option value="Nequi">Nequi</option>
              <option value="Daviplata">Daviplata</option>
              <option value="Otro">Otro</option>
            </select>
          </div>

          <div className="form-group">
            <label>Referencia del Pago</label>
            <input
              type="text"
              className="form-control"
              value={formData.referencia}
              onChange={(e) => setFormData({ ...formData, referencia: e.target.value })}
              placeholder="Número de transacción, cheque, etc."
            />
          </div>

          <div className="form-group">
            <label>Observaciones</label>
            <textarea
              className="form-control"
              rows="3"
              value={formData.observaciones}
              onChange={(e) => setFormData({ ...formData, observaciones: e.target.value })}
              placeholder="Notas adicionales sobre el pago..."
            />
          </div>

          <div style={{ display: 'flex', gap: '10px', marginTop: '20px' }}>
            <button type="submit" disabled={loading} className="btn btn-primary">
              {loading ? 'Guardando...' : 'Registrar Pago'}
            </button>
            <button type="button" onClick={() => navigate('/pagos')} className="btn btn-secondary">
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default CrearPago;
