import axios from 'axios';

const API_BASE_URL = 'https://localhost:5001/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export const cortesApi = {
  getAll: () => api.get('/cortes'),
  getById: (id) => api.get(`/cortes/${id}`),
  create: (data) => api.post('/cortes', data),
};

export const asignacionesApi = {
  getAll: () => api.get('/asignacionestaller'),
  getById: (id) => api.get(`/asignacionestaller/${id}`),
  getByTaller: (tallerId) => api.get(`/asignacionestaller/taller/${tallerId}`),
  create: (data) => api.post('/asignacionestaller', data),
};

export const talleresApi = {
  getAll: () => api.get('/talleres'),
  getById: (id) => api.get(`/talleres/${id}`),
  create: (data) => api.post('/talleres', data),
  update: (id, data) => api.put(`/talleres/${id}`, data),
  delete: (id) => api.delete(`/talleres/${id}`),
};

export const referenciasApi = {
  getAll: () => api.get('/referencias'),
  getById: (id) => api.get(`/referencias/${id}`),
  create: (data) => api.post('/referencias', data),
  update: (id, data) => api.put(`/referencias/${id}`, data),
  delete: (id) => api.delete(`/referencias/${id}`),
};

export const coloresApi = {
  getAll: () => api.get('/colores'),
  create: (data) => api.post('/colores', data),
};

export const tallasApi = {
  getAll: () => api.get('/tallas'),
  create: (data) => api.post('/tallas', data),
};

export const materialesApi = {
  getAll: () => api.get('/materiales'),
  create: (data) => api.post('/materiales', data),
};

export const dashboardApi = {
  getData: () => api.get('/dashboard'),
};

export const avancesApi = {
  getByAsignacion: (asignacionId) => api.get(`/avancestaller/asignacion/${asignacionId}`),
  getUltimo: (asignacionId) => api.get(`/avancestaller/asignacion/${asignacionId}/ultimo`),
  create: (data) => api.post('/avancestaller', data),
};

export const remisionesApi = {
  getAll: () => api.get('/remisiones'),
  getById: (id) => api.get(`/remisiones/${id}`),
  getByAsignacion: (asignacionId) => api.get(`/remisiones/asignacion/${asignacionId}`),
  create: (data) => api.post('/remisiones', data),
  registrarRecepcion: (id, data) => api.put(`/remisiones/${id}/recepcion`, data),
};

export const controlCalidadApi = {
  getById: (id) => api.get(`/controlcalidad/${id}`),
  getByRemision: (remisionId) => api.get(`/controlcalidad/remision/${remisionId}`),
  create: (data) => api.post('/controlcalidad', data),
  actualizarEstado: (id, estado) => api.put(`/controlcalidad/${id}/estado-arreglos`, { estado }),
};

export const pagosApi = {
  getById: (id) => api.get(`/pagos/${id}`),
  getByAsignacion: (asignacionId) => api.get(`/pagos/asignacion/${asignacionId}`),
  getPendientes: () => api.get('/pagos/pendientes'),
  getTotalPagado: (tallerId) => api.get(`/pagos/taller/${tallerId}/total`),
  create: (data) => api.post('/pagos', data),
  actualizarEstado: (id, data) => api.put(`/pagos/${id}/estado`, data),
};

export const inventarioApi = {
  getById: (id) => api.get(`/inventario/${id}`),
  getDisponible: () => api.get('/inventario/disponible'),
  getByReferencia: (referenciaId) => api.get(`/inventario/referencia/${referenciaId}`),
  create: (data) => api.post('/inventario', data),
  actualizarDesdeRemision: (remisionId) => api.post(`/inventario/actualizar-desde-remision/${remisionId}`),
};

export const reportesApi = {
  getPorReferencia: (referenciaId) => api.get(`/reportes/referencia/${referenciaId}`),
  getPorTaller: (tallerId) => api.get(`/reportes/taller/${tallerId}`),
  getFinanciero: (fechaInicio, fechaFin) => api.get('/reportes/financiero', { params: { fechaInicio, fechaFin } }),
  getColores: () => api.get('/reportes/colores'),
};

export const archivosApi = {
  upload: (formData) => {
    return axios.post(`${API_BASE_URL}/archivos/upload`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  },
  download: (nombreArchivo) => api.get(`/archivos/download/${nombreArchivo}`, { responseType: 'blob' }),
  delete: (nombreArchivo) => api.delete(`/archivos/delete/${nombreArchivo}`),
};

export const notificacionesApi = {
  getNoLeidas: () => api.get('/notificaciones/no-leidas'),
  marcarLeida: (id) => api.put(`/notificaciones/${id}/marcar-leida`),
  verificarRetrasos: () => api.post('/notificaciones/verificar-retrasos')
};

export const authApi = {
  login: (email, password) => api.post('/auth/login', { email, password }),
  register: (data) => api.post('/auth/register', data)
};

export default api;
