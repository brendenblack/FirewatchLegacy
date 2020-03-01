import authService from './components/api-authorization/AuthorizeService';

export class BaseClient {
    
    protected transformOptions(options: RequestInit): RequestInit {
        console.log('Transforming options');
        const token = authService.getAccessToken();
        if (token) {
            if (options.headers) {
                console.log('Appending authorization header');
                (options.headers as Headers).append('Authorization', `Bearer ${token}`);
            }
        }
        return options;

    }
}