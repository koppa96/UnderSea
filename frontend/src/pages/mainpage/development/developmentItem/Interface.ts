import { ICreationInfo } from "../../../../api/Client";

export interface DevelopmentProps {
  id?: string;
  imageUrl?: string;
  title?: string;
  description?: string;
  time?: string;
  inProgress?: number;
  count: number;
  info: ICreationInfo | null;
}
