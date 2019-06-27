import { IInclude } from "swagger-axios-codegen/dist/baseInterfaces";

export interface ISwaggerOptions {
  serviceNameSuffix?: string
  enumNamePrefix?: string
  methodNameMode?: 'operationId' | 'path'
  outputDir?: string
  fileName?: string
  remoteUrl?: string
  source?: any
  useStaticMethod?: boolean | undefined
  useCustomerRequestInstance?: boolean | undefined
  include?: Array<string | IInclude>
  format?: (s: string) => string
}
 
const defaultOptions: ISwaggerOptions = {
  serviceNameSuffix: 'Service',
  enumNamePrefix: 'Enum',
  methodNameMode: 'operationId',
  outputDir: 'src\api',
  fileName: 'ClientGen.ts',
  useStaticMethod: true,
  useCustomerRequestInstance: false,
  include: []
}