import { getToken } from "./token";

class Api {
  constructor({baseUrl, headers}) {
    this._baseUrl = baseUrl;
    this._headers = headers;
  }

  authenticate(authData) {
    return fetch(`${this._baseUrl}/User/Login`, {
      method: 'POST',
      headers: this._headers,
      body: JSON.stringify({
        login: authData.login,
        password: authData.password,
      }),
    })
    .then(this._checkJsonResponse);
  }

  register(regData) {
    return fetch(`${this._baseUrl}/User/Register`, {
      method: 'POST',
      headers: this._headers,
      body: JSON.stringify({
        email: regData.email,
        login: regData.login,
        name: regData.name,
        password: regData.password,
        surname: regData.surname,
      }),
    })
    .then(this._checkJsonResponse);
  }

  getUserData(login) {
    return fetch(`${this._baseUrl}/User?userLogin=${login}`, {
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${getToken()}`,
      }
    })
    .then(this._checkJsonResponse);
  }

  uploadFile(login, formData) {
    return fetch(`${this._baseUrl}/File/Upload?userLogin=${login}`, {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${getToken()}`,
      },
      body: formData,
    })
    .then(this._checkJsonResponse);
  }

  getFile(id) {
    return fetch(`${this._baseUrl}/File/File?fileId=${id}`, {
      headers: {
        'Authorization': `Bearer ${getToken()}`,
      }
    })
    .then(this._checkBlobResponse);
  }

  getProviders(id) {
    return fetch(`${this._baseUrl}/File/Providers?fileId=${id}`, {
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${getToken()}`,
      }
    })
    .then(this._checkJsonResponse);
  }

  deleteFile(id) {
    return fetch(`${this._baseUrl}/File?fileId=${id}`, {
      method: 'DELETE',
      headers: {
        'Authorization': `Bearer ${getToken()}`,
      }
    })
    .then(this._checkJsonResponse);
  }

  generateReport(fileId, format) {
    return fetch(`${this._baseUrl}/Report/Generate?fileId=${fileId}&&format=${format}`, {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${getToken()}`,
      }
    })
    .then(this._checkJsonResponse);
  }

  generateReportWithProvider(fileId, format, provider) {
    return fetch(`${this._baseUrl}/Report/GenerateWithProvider?fileId=${fileId}&&format=${format}&&provider=${provider}`, {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${getToken()}`,
      }
    })
    .then(this._checkJsonResponse);
  }

  getReportFile(id) {
    return fetch(`${this._baseUrl}/Report/File?reportId=${id}`, {
      headers: {
        'Authorization': `Bearer ${getToken()}`,
      }
    })
    .then(this._checkBlobResponse);
  }

  _checkBlobResponse(res) {
    if (res.ok) {
      return res.blob();
    }

    return Promise.reject(`Ошибка: ${res.status}`);
  }

  _checkJsonResponse(res) {
    if (res.ok) {
      return res.json();
    }
    
    return Promise.reject(`Ошибка: ${res.status}`); 
  }
}

export const api = new Api({
  baseUrl: 'https://localhost:44366/api',
  headers: {
    'Content-Type': 'application/json',
  }
})