/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface AddressModel {
  coordinates?: CoordinatesModel;
  description?: string | null;
}

export interface AppointmentCreatedResponse {
  /** @format uuid */
  appointmentId?: string;
}

export interface AppointmentCustomerModel {
  /** @format uuid */
  customerId?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  email?: string | null;
  phoneNumber?: string | null;
  profileImage?: string | null;
}

export interface AppointmentListResponse {
  /** @format uuid */
  id?: string;
  /** @format date-time */
  createdAt?: string;
  createdBy?: string | null;
  /** @format date-time */
  updatedAt?: string | null;
  updatedBy?: string | null;
  /** @format uuid */
  tenantId?: string;
  /** @format uuid */
  branchOfficeId?: string;
  /** @format uuid */
  serviceId?: string;
  /** @format uuid */
  serviceProviderId?: string | null;
  /** @format date-time */
  appointmentDate?: string;
  appointmentCode?: string | null;
  status?: string | null;
}

export interface AppointmentListResponsePagedResult {
  /** @format int32 */
  pageIndex?: number;
  /** @format int32 */
  pageSize?: number;
  /** @format int64 */
  totalCount?: number;
  results?: AppointmentListResponse[] | null;
}

export interface AppointmentProfileResponse {
  /** @format uuid */
  id?: string;
  /** @format date-time */
  createdAt?: string;
  createdBy?: string | null;
  /** @format date-time */
  updatedAt?: string | null;
  updatedBy?: string | null;
  /** @format uuid */
  tenantId?: string;
  /** @format uuid */
  branchOfficeId?: string;
  /** @format uuid */
  serviceId?: string;
  /** @format uuid */
  serviceProviderId?: string | null;
  /** @format date-time */
  appointmentDate?: string;
  appointmentCode?: string | null;
  customer?: AppointmentCustomerModel;
  notes?: string | null;
  status?: string | null;
}

export interface AssetCreatedResponse {
  /** @format uuid */
  id?: string;
  transactionCode?: string | null;
}

export interface AssetResponse {
  /** @format uuid */
  id?: string;
  /** @format date-time */
  createdAt?: string;
  createdBy?: string | null;
  /** @format date-time */
  updatedAt?: string | null;
  updatedBy?: string | null;
  path?: string | null;
  contentType?: string | null;
  transaction?: AssetTransactionResponse;
}

export interface AssetTransactionResponse {
  status?: string | null;
  code?: string | null;
  /** @format date-time */
  expiresAt?: string | null;
}

export interface AssetUploadResponse {
  transactionStatus?: string | null;
}

export interface BranchOfficeCreatedResponse {
  /** @format uuid */
  branchOfficeId?: string;
}

export interface BranchOfficeListResponse {
  /** @format uuid */
  id?: string;
  /** @format date-time */
  createdAt?: string;
  createdBy?: string | null;
  /** @format date-time */
  updatedAt?: string | null;
  updatedBy?: string | null;
  /** @format uuid */
  tenantId?: string;
  name?: string | null;
  address?: AddressModel;
}

export interface BranchOfficeListResponsePagedResult {
  /** @format int32 */
  pageIndex?: number;
  /** @format int32 */
  pageSize?: number;
  /** @format int64 */
  totalCount?: number;
  results?: BranchOfficeListResponse[] | null;
}

export interface BranchOfficeProfileResponse {
  /** @format uuid */
  id?: string;
  /** @format date-time */
  createdAt?: string;
  createdBy?: string | null;
  /** @format date-time */
  updatedAt?: string | null;
  updatedBy?: string | null;
  /** @format uuid */
  tenantId?: string;
  name?: string | null;
  address?: AddressModel;
  contacts?: SocialMediaContactModel[] | null;
  schedule?: WeeklyScheduleModel;
}

export interface CoordinatesModel {
  /** @format int32 */
  longitude?: number;
  /** @format int32 */
  latitude?: number;
}

export interface CreateAppointmentRequest {
  /** @format uuid */
  branchOfficeId?: string;
  /** @format uuid */
  serviceId?: string;
  /** @format uuid */
  serviceProviderId?: string | null;
  /** @format date-time */
  appointmentDate?: string;
  customer?: AppointmentCustomerModel;
  notes?: string | null;
}

export interface CreateAssetRequest {
  path?: string | null;
  contentType?: string | null;
}

export interface CreateBranchOfficeRequest {
  name?: string | null;
  address?: AddressModel;
  contacts?: SocialMediaContactModel[] | null;
  schedule?: WeeklyScheduleModel;
}

export interface CreateServiceRequest {
  name?: string | null;
  description?: string | null;
  /** @format double */
  price?: number;
  images?: IndexedResourceModel[] | null;
  termsAndConditions?: string[] | null;
  notes?: string | null;
  customerDuration?: TimeSpan;
  calendarDuration?: TimeSpan;
}

export interface DailySchedule {
  hours?: DateRange[] | null;
  disabled?: boolean;
}

export interface DailyScheduleModel {
  hours?: DateRangeModel[] | null;
  disabled?: boolean;
}

export interface DateRange {
  /** @format date-time */
  readonly startDate?: string;
  /** @format date-time */
  readonly endDate?: string;
  readonly disabled?: boolean;
}

export interface DateRangeModel {
  /** @format date-time */
  startDate?: string;
  /** @format date-time */
  endDate?: string;
  disabled?: boolean;
}

export interface IndexedResourceModel {
  /** @format int32 */
  index?: number;
  path?: string | null;
}

export interface LoginWithEmailRequest {
  email?: string | null;
  password?: string | null;
  scope?: string | null;
  /** @format uuid */
  tenantId?: string | null;
}

export interface OAuthTokenResponse {
  access_token?: string | null;
  token_type?: string | null;
  /** @format int32 */
  expires_in?: number | null;
  scope?: string | null;
  id_token?: string | null;
  refresh_token?: string | null;
}

export interface ServiceCreatedResponse {
  /** @format uuid */
  serviceId?: string;
}

export interface ServiceListResponse {
  /** @format uuid */
  id?: string;
  /** @format date-time */
  createdAt?: string;
  createdBy?: string | null;
  /** @format date-time */
  updatedAt?: string | null;
  updatedBy?: string | null;
  /** @format uuid */
  tenantId?: string;
  name?: string | null;
  description?: string | null;
  /** @format double */
  price?: number;
  customerDuration?: TimeSpan;
  calendarDuration?: TimeSpan;
}

export interface ServiceListResponsePagedResult {
  /** @format int32 */
  pageIndex?: number;
  /** @format int32 */
  pageSize?: number;
  /** @format int64 */
  totalCount?: number;
  results?: ServiceListResponse[] | null;
}

export interface ServiceProfileResponse {
  /** @format uuid */
  id?: string;
  /** @format date-time */
  createdAt?: string;
  createdBy?: string | null;
  /** @format date-time */
  updatedAt?: string | null;
  updatedBy?: string | null;
  /** @format uuid */
  tenantId?: string;
  name?: string | null;
  description?: string | null;
  /** @format double */
  price?: number;
  images?: IndexedResourceModel[] | null;
  termsAndConditions?: string[] | null;
  notes?: string | null;
  customerDuration?: TimeSpan;
  calendarDuration?: TimeSpan;
}

export interface SetAppointmentStatusRequest {
  status?: string | null;
}

export interface SignUpWithEmailRequest {
  firstName?: string | null;
  lastName?: string | null;
  email?: string | null;
  password?: string | null;
  passwordConfirm?: string | null;
  companyName?: string | null;
}

export interface SocialMediaContact {
  type?: SocialMediaType;
  otherType?: string | null;
  contact?: string | null;
}

export interface SocialMediaContactModel {
  type?: string | null;
  otherType?: string | null;
  contact?: string | null;
}

/** @format int32 */
export type SocialMediaType = 0 | 1 | 2 | 3 | 4;

export interface TenantProfileResponse {
  /** @format uuid */
  id?: string;
  /** @format date-time */
  createdAt?: string;
  createdBy?: string | null;
  /** @format date-time */
  updatedAt?: string | null;
  updatedBy?: string | null;
  name?: string | null;
  slogan?: string | null;
  urlId?: string | null;
  logo?: string | null;
  contacts?: SocialMediaContact[] | null;
  schedule?: WeeklySchedule;
}

export interface TenantUrlId {
  readonly value?: string | null;
}

export interface TimeSpan {
  /** @format int64 */
  ticks?: number;
  /** @format int32 */
  readonly days?: number;
  /** @format int32 */
  readonly hours?: number;
  /** @format int32 */
  readonly milliseconds?: number;
  /** @format int32 */
  readonly microseconds?: number;
  /** @format int32 */
  readonly nanoseconds?: number;
  /** @format int32 */
  readonly minutes?: number;
  /** @format int32 */
  readonly seconds?: number;
  /** @format double */
  readonly totalDays?: number;
  /** @format double */
  readonly totalHours?: number;
  /** @format double */
  readonly totalMilliseconds?: number;
  /** @format double */
  readonly totalMicroseconds?: number;
  /** @format double */
  readonly totalNanoseconds?: number;
  /** @format double */
  readonly totalMinutes?: number;
  /** @format double */
  readonly totalSeconds?: number;
}

export interface UpdateBranchOfficeRequest {
  name?: string | null;
  address?: AddressModel;
  contacts?: SocialMediaContactModel[] | null;
  schedule?: WeeklyScheduleModel;
}

export interface UpdateServiceRequest {
  name?: string | null;
  description?: string | null;
  /** @format double */
  price?: number;
  images?: IndexedResourceModel[] | null;
  termsAndConditions?: string[] | null;
  notes?: string | null;
  customerDuration?: TimeSpan;
  calendarDuration?: TimeSpan;
}

export interface UpdateTenantProfileRequest {
  updatedBy?: string | null;
  /** @format uuid */
  id?: string;
  name?: string | null;
  slogan?: string | null;
  urlId?: TenantUrlId;
  contacts?: SocialMediaContact[] | null;
}

export interface UpdateUserProfileRequest {
  firstName?: string | null;
  lastName?: string | null;
}

export interface UserLoginResponse {
  identityProvider?: string | null;
  email?: string | null;
  phoneNumber?: string | null;
}

export interface UserPreferenceResponse {
  key?: string | null;
  value?: string | null;
}

export interface UserProfileResponse {
  /** @format uuid */
  id?: string;
  /** @format date-time */
  createdAt?: string;
  createdBy?: string | null;
  /** @format date-time */
  updatedAt?: string | null;
  updatedBy?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  profileImage?: string | null;
  logins?: UserLoginResponse[] | null;
  tenants?: UserTenantResponse[] | null;
  preferences?: UserPreferenceResponse[] | null;
}

export interface UserSignedUpResponse {
  /** @format uuid */
  userId?: string;
  /** @format uuid */
  tenantId?: string;
}

export interface UserTenantResponse {
  /** @format uuid */
  tenantId?: string;
  tenantName?: string | null;
}

export interface WeeklySchedule {
  monday?: DailySchedule;
  tuesday?: DailySchedule;
  wednesday?: DailySchedule;
  thursday?: DailySchedule;
  friday?: DailySchedule;
  saturday?: DailySchedule;
  sunday?: DailySchedule;
}

export interface WeeklyScheduleModel {
  monday?: DailyScheduleModel;
  tuesday?: DailyScheduleModel;
  wednesday?: DailyScheduleModel;
  thursday?: DailyScheduleModel;
  friday?: DailyScheduleModel;
  saturday?: DailyScheduleModel;
  sunday?: DailyScheduleModel;
}

export type QueryParamsType = Record<string | number, any>;
export type ResponseFormat = keyof Omit<Body, "body" | "bodyUsed">;

export interface FullRequestParams extends Omit<RequestInit, "body"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseFormat;
  /** request body */
  body?: unknown;
  /** base url */
  baseUrl?: string;
  /** request cancellation token */
  cancelToken?: CancelToken;
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">;

export interface ApiConfig<SecurityDataType = unknown> {
  baseUrl?: string;
  baseApiParams?: Omit<RequestParams, "baseUrl" | "cancelToken" | "signal">;
  securityWorker?: (securityData: SecurityDataType | null) => Promise<RequestParams | void> | RequestParams | void;
  customFetch?: typeof fetch;
}

export interface HttpResponse<D extends unknown, E extends unknown = unknown> extends Response {
  data: D;
  error: E;
}

type CancelToken = Symbol | string | number;

export enum ContentType {
  Json = "application/json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public baseUrl: string = "/";
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private abortControllers = new Map<CancelToken, AbortController>();
  private customFetch = (...fetchParams: Parameters<typeof fetch>) => fetch(...fetchParams);

  private baseApiParams: RequestParams = {
    credentials: "same-origin",
    headers: {},
    redirect: "follow",
    referrerPolicy: "no-referrer",
  };

  constructor(apiConfig: ApiConfig<SecurityDataType> = {}) {
    Object.assign(this, apiConfig);
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected encodeQueryParam(key: string, value: any) {
    const encodedKey = encodeURIComponent(key);
    return `${encodedKey}=${encodeURIComponent(typeof value === "number" ? value : `${value}`)}`;
  }

  protected addQueryParam(query: QueryParamsType, key: string) {
    return this.encodeQueryParam(key, query[key]);
  }

  protected addArrayQueryParam(query: QueryParamsType, key: string) {
    const value = query[key];
    return value.map((v: any) => this.encodeQueryParam(key, v)).join("&");
  }

  protected toQueryString(rawQuery?: QueryParamsType): string {
    const query = rawQuery || {};
    const keys = Object.keys(query).filter((key) => "undefined" !== typeof query[key]);
    return keys
      .map((key) => (Array.isArray(query[key]) ? this.addArrayQueryParam(query, key) : this.addQueryParam(query, key)))
      .join("&");
  }

  protected addQueryParams(rawQuery?: QueryParamsType): string {
    const queryString = this.toQueryString(rawQuery);
    return queryString ? `?${queryString}` : "";
  }

  private contentFormatters: Record<ContentType, (input: any) => any> = {
    [ContentType.Json]: (input: any) =>
      input !== null && (typeof input === "object" || typeof input === "string") ? JSON.stringify(input) : input,
    [ContentType.Text]: (input: any) => (input !== null && typeof input !== "string" ? JSON.stringify(input) : input),
    [ContentType.FormData]: (input: any) =>
      Object.keys(input || {}).reduce((formData, key) => {
        const property = input[key];
        formData.append(
          key,
          property instanceof Blob
            ? property
            : typeof property === "object" && property !== null
            ? JSON.stringify(property)
            : `${property}`,
        );
        return formData;
      }, new FormData()),
    [ContentType.UrlEncoded]: (input: any) => this.toQueryString(input),
  };

  protected mergeRequestParams(params1: RequestParams, params2?: RequestParams): RequestParams {
    return {
      ...this.baseApiParams,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...(this.baseApiParams.headers || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected createAbortSignal = (cancelToken: CancelToken): AbortSignal | undefined => {
    if (this.abortControllers.has(cancelToken)) {
      const abortController = this.abortControllers.get(cancelToken);
      if (abortController) {
        return abortController.signal;
      }
      return void 0;
    }

    const abortController = new AbortController();
    this.abortControllers.set(cancelToken, abortController);
    return abortController.signal;
  };

  public abortRequest = (cancelToken: CancelToken) => {
    const abortController = this.abortControllers.get(cancelToken);

    if (abortController) {
      abortController.abort();
      this.abortControllers.delete(cancelToken);
    }
  };

  public request = async <T = any, E = any>({
    body,
    secure,
    path,
    type,
    query,
    format,
    baseUrl,
    cancelToken,
    ...params
  }: FullRequestParams): Promise<HttpResponse<T, E>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.baseApiParams.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const queryString = query && this.toQueryString(query);
    const payloadFormatter = this.contentFormatters[type || ContentType.Json];
    const responseFormat = format || requestParams.format;

    return this.customFetch(`${baseUrl || this.baseUrl || ""}${path}${queryString ? `?${queryString}` : ""}`, {
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type && type !== ContentType.FormData ? { "Content-Type": type } : {}),
      },
      signal: (cancelToken ? this.createAbortSignal(cancelToken) : requestParams.signal) || null,
      body: typeof body === "undefined" || body === null ? null : payloadFormatter(body),
    }).then(async (response) => {
      const r = response as HttpResponse<T, E>;
      r.data = null as unknown as T;
      r.error = null as unknown as E;

      const data = !responseFormat
        ? r
        : await response[responseFormat]()
            .then((data) => {
              if (r.ok) {
                r.data = data;
              } else {
                r.error = data;
              }
              return r;
            })
            .catch((e) => {
              r.error = e;
              return r;
            });

      if (cancelToken) {
        this.abortControllers.delete(cancelToken);
      }

      if (!response.ok) throw data;
      return data;
    });
  };
}

/**
 * @title Appointments.Api.Client
 * @version 1.0
 * @baseUrl /
 */
export class AppointmentsApiClient<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
  assets = {
    /**
     * No description
     *
     * @tags Assets
     * @name CreateAsset
     * @request POST:/api/assets
     * @secure
     */
    createAsset: (data: CreateAssetRequest, params: RequestParams = {}) =>
      this.request<AssetCreatedResponse, any>({
        path: `/api/assets`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Assets
     * @name GetAsset
     * @request GET:/api/assets/{id}
     * @secure
     */
    getAsset: (id: string, params: RequestParams = {}) =>
      this.request<AssetResponse, any>({
        path: `/api/assets/${id}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Assets
     * @name UploadAsset
     * @request POST:/api/assets/blob/{path}
     * @secure
     */
    uploadAsset: (
      path: string,
      data: {
        ContentType?: string;
        ContentDisposition?: string;
        Headers?: Record<string, string[]>;
        /** @format int64 */
        Length?: number;
        Name?: string;
        FileName?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<AssetUploadResponse, any>({
        path: `/api/assets/blob/${path}`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.FormData,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Assets
     * @name ServeAsset
     * @request GET:/api/assets/blob/{path}
     * @secure
     */
    serveAsset: (path: string, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/assets/blob/${path}`,
        method: "GET",
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Assets
     * @name DeleteAsset
     * @request DELETE:/api/assets/blob/{path}
     * @secure
     */
    deleteAsset: (path: string, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/assets/blob/${path}`,
        method: "DELETE",
        secure: true,
        ...params,
      }),
  };
  connect = {
    /**
     * No description
     *
     * @tags Connect
     * @name SignUpWithEmail
     * @request POST:/api/connect/sign-up/email
     * @secure
     */
    signUpWithEmail: (data: SignUpWithEmailRequest, params: RequestParams = {}) =>
      this.request<UserSignedUpResponse, any>({
        path: `/api/connect/sign-up/email`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Connect
     * @name LoginWithEmail
     * @request POST:/api/connect/login/email
     * @secure
     */
    loginWithEmail: (data: LoginWithEmailRequest, params: RequestParams = {}) =>
      this.request<OAuthTokenResponse, any>({
        path: `/api/connect/login/email`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
  me = {
    /**
     * No description
     *
     * @tags Me
     * @name GetMyProfile
     * @request GET:/api/tenant/me
     * @secure
     */
    getMyProfile: (params: RequestParams = {}) =>
      this.request<UserProfileResponse, any>({
        path: `/api/tenant/me`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Me
     * @name UpdateMyProfile
     * @request PUT:/api/tenant/me
     * @secure
     */
    updateMyProfile: (data: UpdateUserProfileRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/tenant/me`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),
  };
  tenants = {
    /**
     * No description
     *
     * @tags Tenants
     * @name GetTenant
     * @request GET:/api/tenant/tenants/{tenantId}
     * @secure
     */
    getTenant: (tenantId: string, params: RequestParams = {}) =>
      this.request<TenantProfileResponse, any>({
        path: `/api/tenant/tenants/${tenantId}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name UpdateTenantProfile
     * @request PUT:/api/tenant/tenants/{tenantId}
     * @secure
     */
    updateTenantProfile: (tenantId: string, data: UpdateTenantProfileRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/tenant/tenants/${tenantId}`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name CreateBranchOffice
     * @request POST:/api/tenant/tenants/{tenantId}/branch-offices
     * @secure
     */
    createBranchOffice: (tenantId: string, data: CreateBranchOfficeRequest, params: RequestParams = {}) =>
      this.request<BranchOfficeCreatedResponse, any>({
        path: `/api/tenant/tenants/${tenantId}/branch-offices`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name FindBranchOffices
     * @request GET:/api/tenant/tenants/{tenantId}/branch-offices
     * @secure
     */
    findBranchOffices: (
      tenantId: string,
      query?: {
        /**
         * @format int32
         * @default 0
         */
        pageIndex?: number;
        /**
         * @format int32
         * @default 10
         */
        pageSize?: number;
        sort?: string;
        filter?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<BranchOfficeListResponsePagedResult, any>({
        path: `/api/tenant/tenants/${tenantId}/branch-offices`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name GetBranchOffice
     * @request GET:/api/tenant/tenants/{tenantId}/branch-offices/{branchOfficeId}
     * @secure
     */
    getBranchOffice: (tenantId: string, branchOfficeId: string, params: RequestParams = {}) =>
      this.request<BranchOfficeProfileResponse, any>({
        path: `/api/tenant/tenants/${tenantId}/branch-offices/${branchOfficeId}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name UpdateBranchOffice
     * @request PUT:/api/tenant/tenants/{tenantId}/branch-offices/{branchOfficeId}
     * @secure
     */
    updateBranchOffice: (
      tenantId: string,
      branchOfficeId: string,
      data: UpdateBranchOfficeRequest,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/tenant/tenants/${tenantId}/branch-offices/${branchOfficeId}`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name DeleteBranchOffice
     * @request DELETE:/api/tenant/tenants/{tenantId}/branch-offices/{branchOfficeId}
     * @secure
     */
    deleteBranchOffice: (tenantId: string, branchOfficeId: string, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/tenant/tenants/${tenantId}/branch-offices/${branchOfficeId}`,
        method: "DELETE",
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name CreateService
     * @request POST:/api/tenant/tenants/{tenantId}/services
     * @secure
     */
    createService: (tenantId: string, data: CreateServiceRequest, params: RequestParams = {}) =>
      this.request<ServiceCreatedResponse, any>({
        path: `/api/tenant/tenants/${tenantId}/services`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name FindServices
     * @request GET:/api/tenant/tenants/{tenantId}/services
     * @secure
     */
    findServices: (
      tenantId: string,
      query?: {
        /**
         * @format int32
         * @default 0
         */
        pageIndex?: number;
        /**
         * @format int32
         * @default 10
         */
        pageSize?: number;
        sort?: string;
        filter?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<ServiceListResponsePagedResult, any>({
        path: `/api/tenant/tenants/${tenantId}/services`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name GetService
     * @request GET:/api/tenant/tenants/{tenantId}/services/{serviceId}
     * @secure
     */
    getService: (tenantId: string, serviceId: string, params: RequestParams = {}) =>
      this.request<ServiceProfileResponse, any>({
        path: `/api/tenant/tenants/${tenantId}/services/${serviceId}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name UpdateService
     * @request PUT:/api/tenant/tenants/{tenantId}/services/{serviceId}
     * @secure
     */
    updateService: (tenantId: string, serviceId: string, data: UpdateServiceRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/tenant/tenants/${tenantId}/services/${serviceId}`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name DeleteService
     * @request DELETE:/api/tenant/tenants/{tenantId}/services/{serviceId}
     * @secure
     */
    deleteService: (tenantId: string, serviceId: string, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/tenant/tenants/${tenantId}/services/${serviceId}`,
        method: "DELETE",
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name CreateAppointment
     * @request POST:/api/tenant/tenants/{tenantId}/appointments
     * @secure
     */
    createAppointment: (tenantId: string, data: CreateAppointmentRequest, params: RequestParams = {}) =>
      this.request<AppointmentCreatedResponse, any>({
        path: `/api/tenant/tenants/${tenantId}/appointments`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name FindAppointments
     * @request GET:/api/tenant/tenants/{tenantId}/appointments
     * @secure
     */
    findAppointments: (
      tenantId: string,
      query?: {
        /**
         * @format int32
         * @default 0
         */
        pageIndex?: number;
        /**
         * @format int32
         * @default 10
         */
        pageSize?: number;
        sort?: string;
        filter?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<AppointmentListResponsePagedResult, any>({
        path: `/api/tenant/tenants/${tenantId}/appointments`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name GetAppointment
     * @request GET:/api/tenant/tenants/{tenantId}/appointments/{appointmentId}
     * @secure
     */
    getAppointment: (tenantId: string, appointmentId: string, params: RequestParams = {}) =>
      this.request<AppointmentProfileResponse, any>({
        path: `/api/tenant/tenants/${tenantId}/appointments/${appointmentId}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name DeleteAppointment
     * @request DELETE:/api/tenant/tenants/{tenantId}/appointments/{appointmentId}
     * @secure
     */
    deleteAppointment: (tenantId: string, appointmentId: string, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/tenant/tenants/${tenantId}/appointments/${appointmentId}`,
        method: "DELETE",
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Tenants
     * @name SetAppointmentStatus
     * @request PUT:/api/tenant/tenants/{tenantId}/appointments/{appointmentId}/status
     * @secure
     */
    setAppointmentStatus: (
      tenantId: string,
      appointmentId: string,
      data: SetAppointmentStatusRequest,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/tenant/tenants/${tenantId}/appointments/${appointmentId}/status`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),
  };
}
