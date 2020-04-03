import authService from './components/api-authorization/AuthorizeService';

export class BaseClient {
    
    protected transformOptions(options: RequestInit): Promise<RequestInit> {
        // console.log('Transforming options');
        return authService.getAccessToken()
            .then(token => {
                if (token) {
                    if (options.headers) {
                        // console.log('Appending authorization header');
                        // (options.headers as Headers).append('Authorization', `Bearer ${token}`);
                        (options.headers as Record<string,string>)['Authorization'] = `Bearer ${token}`;
                        // console.log('Transformed headers', options.headers);
                    }
                }
                return options;
                // return new Promise(resolve => { return options; });
            });
        

    }
}