import { HttpHeaders, HttpRequest } from "@angular/common/http";

export default class Utils {
    static getHttpOptionsHeader(token: any) {
        if (token === null || token === undefined) {
            return null;
        }

        const httpOptions = {
            headers: new HttpHeaders({
                'Authorization': 'Bearer ' + token
        })
        };
        
        return httpOptions;
    }
}
