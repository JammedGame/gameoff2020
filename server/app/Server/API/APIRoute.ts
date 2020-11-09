export enum APIRouteType
{
    GET = 'get',
    PUT = 'put',
    POST = 'post'
}

export type APIResponse = {
    status: number,
    body: any
}

export class APIRoute
{
    private expressApp: any;
    private callback: Function;
    private type: APIRouteType;
    public constructor(expressApp: any, type: APIRouteType, route: string, callback: Function)
    {
        this.type = type;
        this.callback = callback;
        this.expressApp = expressApp;
        this.expressApp[type](route, (req, res) => this.handleRequest(req, res))
    }
    public handleRequest(req, res): void
    {
        const params = (this.type === APIRouteType.GET) ? req.query : req.body;
        const response = this.callback(params);
        res.status(response.status).json(response.body);
    }
}
