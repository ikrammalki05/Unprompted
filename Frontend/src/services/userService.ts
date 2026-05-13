import api from './api';

const userService = {
  // LOGIN
  login: async (email: string, password: string) => {
    const response = await api.post('/auth/login', {
      email,
      password,
    });

    if (response.data.token) {
      localStorage.setItem('token', response.data.token);
    }

    return response.data;
  },

  // CURRENT USER (Enseignant)
  getCurrentUser: async () => {
    const response = await api.get('/enseignant/me');
    return response.data;
  },

  // STUDENTS
  getStudents: async () => {
    const response = await api.get('/etudiant');
    return response.data;
  },

  // ENSEIGNANT STATS
  getTeacherStats: async (enseignantId: number) => {
    const response = await api.get(
      `/enseignant/${enseignantId}/statistiques`
    );

    return response.data;
  },

  // UPDATE CURRENT USER
  updateMyProfile: async (data: any) => {
    const response = await api.put('/enseignant/me', data);
    return response.data;
  },

  // LOGOUT
  logout: () => {
    localStorage.removeItem('token');
  },
};

export default userService;