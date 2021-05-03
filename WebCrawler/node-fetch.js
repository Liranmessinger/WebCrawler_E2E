const fetch = require("node-fetch");
const url = "https://github.com/";

async function getData(url) {
    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.text();
    } catch (error) {
        console.log(error);
    }
};

module.exports = { getData };

