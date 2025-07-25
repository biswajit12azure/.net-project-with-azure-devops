import { store, authActions } from '_store';

export const fetchWrapper = {
    get: request('GET'),
    post: request('POST'),
    put: request('PUT'),
    delete: request('DELETE')
};

function request(method) {
    return (url, body) => {
        const requestOptions = {
            method,
            headers: authHeader(url)
        };
        if (body) {
            requestOptions.headers['Content-Type'] = 'application/json';
            requestOptions.body = JSON.stringify(body);
        }
        return fetch(url, requestOptions).then(handleResponse);
    }
}

// helper functions

function authHeader(url) {
    // return auth header with jwt if user is logged in and request is to the api url
    const token = authToken();
    const isLoggedIn = !!token;
    const isApiUrl = url.startsWith(process.env.REACT_APP_API_URL) || url.startsWith(process.env.REACT_APP_MARKETER_API_URL);
    if (isLoggedIn && isApiUrl) {
        return { Authorization: `Bearer ${token}` };
    } else {
        return {};
    }
}

function authToken() {
    if (store.getState().auth.value) {
        const { Data: { UserDetails: { jwToken } } } = store.getState().auth.value;
        return jwToken;
    }
    return store.getState().auth.value;
}

async function handleResponse(response) {
    const isJson = response.headers?.get('content-type')?.includes('application/json');
    const data = isJson ? await response.json() : null;

    // check for error response
    if (!response.ok) {
        if ([401, 403].includes(response.status) && authToken()) {
            // auto logout if 401 Unauthorized or 403 Forbidden response returned from api
            const logout = () => store.dispatch(authActions.logout());
            logout();
        }

        
         // handle validation errors
         if (response.status === 400) {
            const errorData= !isJson && await response.json() ;
            const validationErrors =  errorData && errorData?.errors;
            const errors= (data && data.Message) || JSON.stringify(validationErrors);
            return Promise.reject(errors);
        }

        // get error message from body or default to response status
        const error = (data && data.Message) || response.statusText;
        return Promise.reject(error);
    }

    return data;
}