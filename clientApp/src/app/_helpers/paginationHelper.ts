import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs";
import { PaginatedResult } from "../_models/pagination";

export function getPaginatedResults<T>(url, params, http: HttpClient){
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return http.get<T>(url, {observe: 'response', params}).pipe(
      map(response => {
        paginatedResult.results = response.body;
        if(response.headers.get('Pagination-Info') !== null){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination-Info'));
        }
        return paginatedResult;
      })
    )
  }

  export function getPaginatedHeaders(pageNumber: number, pageSize: number){
    let params = new HttpParams();  
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    
    return params;
  }