import { getLocaleDateFormat } from "@angular/common";
import { DateLocaleConstants } from "../reference/dateLocaleConstants";

export class LocalDateFormatter
{
    public static showDateAsDDMMYYYY(value: Date)
    {
        const newDate = new Date(value);
        return newDate.toLocaleDateString(DateLocaleConstants.EN_GB_Language);
    }
    public static showDateAsMMDDYYYY(value: Date)
    {
        const newDate = new Date(value);
        return newDate.toLocaleDateString(DateLocaleConstants.EN_US_Language);
    }
}