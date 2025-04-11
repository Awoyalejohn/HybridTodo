export async function loginAsync(email, password) {
    const url = "/api/auth/login";
    var data = {
        email: email,
        password: password
    };
    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    };
    try {
        const response = await fetch(url, options);
        if (!response.ok) {
            return null;
        }
        else {
            return await response.json();
        }
    }
    catch (error) {
        return null;
    }
}
export async function logoutAsync() {
    const url = "/api/auth/logout";
    await fetch(url, { method: 'POST' });
}
//# sourceMappingURL=auth.js.map