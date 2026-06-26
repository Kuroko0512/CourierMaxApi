namespace Dominio.Servicios
{
    public static class DiasHabiles
    {
        public static int Transcurridos(DateTime desde, DateTime hasta, IReadOnlySet<DateOnly> festivos)
        {
            var dias = 0;

            for (var d = DateOnly.FromDateTime(desde).AddDays(1); d <= DateOnly.FromDateTime(hasta); d = d.AddDays(1))
            {
                if (d.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
                {
                    continue;
                }

                if (festivos.Contains(d))
                {
                    continue;
                }

                dias++;
            }

            return dias;
        }
    }
}
