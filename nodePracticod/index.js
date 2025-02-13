const express = require('express');
const app = express();
const PORT = 3000;
const renderKey = process.env.RENDER_API_KEY;

app.get('/', async (req, res) => {

    const renderApi = require('api')('@render-api/v1.0#5yi85d37lhqedd36');
    renderApi.auth(renderKey);
    renderApi.getServices({includePreviews: 'true',limit: '20' })
        .then(({ data }) => res.send(data))
        .catch(err => console.error(err));

});

app.listen(PORT, () => {
    console.log(`Server is running on port ${PORT}`);
});
