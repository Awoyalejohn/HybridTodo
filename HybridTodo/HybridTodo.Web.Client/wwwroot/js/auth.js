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
            return false;
        }
        else {
            return true;
        }
    }
    catch (error) {
        return false;
    }
}
export async function logoutAsync() {
    const url = "/api/auth/logout";
    await fetch(url, { method: 'POST' });
}
//# sourceMappingURL=auth.js.map