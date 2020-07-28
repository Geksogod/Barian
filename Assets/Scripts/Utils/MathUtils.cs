public class MathUtils
{
    public static float Normalize(float input, float input_min, float input_max, float output_min, float output_max)
    {
        return (output_min * (input - input_max) - output_max * (input - input_min)) / (input_min - input_max);
    }
}
