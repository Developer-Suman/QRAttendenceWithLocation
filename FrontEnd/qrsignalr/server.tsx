/* eslint-disable @typescript-eslint/no-require-imports */
const { createServer } = require("http");
const next = require("next");
const { parse } = require("url");

const port = process.env.PORT || 3000;
const dev = process.env.NODE_ENV !== "production";
const app = next({ dev });
const handle = app.getRequestHandler();

app.prepare().then(() => {
  createServer((req: any, res: any) => {
    const parsedUrl = parse(req.url, true);
    handle(req, res, parsedUrl);
  }).listen(port, (err: any) => {
    if (err) throw err;
    console.log(`> Ready on http://localhost:${port}`);
  });
});
