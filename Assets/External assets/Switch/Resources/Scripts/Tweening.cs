/// See http://wiki.unity3d.com/index.php?title=Tween
using UnityEngine;

namespace Tween
{
    public enum Ease
    {
        None = -1,
        Linear = 0,
        EaseInQuad = 1,
        EaseOutQuad = 2,
        EaseInOutQuad = 3,
        EaseOutInQuad = 4,
        EaseInCubic = 5,
        EaseOutCubic = 6,
        EaseInOutCubic = 7,
        EaseOutInCubic = 8,
        EaseInQuart = 9,
        EaseOutQuart = 10,
        EaseInOutQuart = 11,
        EaseOutInQuart = 12,
        EaseInQuint = 13,
        EaseOutQuint = 14,
        EaseInOutQuint = 15,
        EaseOutInQuint = 16,
        EaseInSine = 17,
        EaseOutSine = 18,
        EaseInOutSine = 19,
        EaseOutInSine = 20,
        EaseInExpo = 21,
        EaseOutExpo = 22,
        EaseInOutExpo = 23,
        EaseOutInExpo = 24,
        EaseInCirc = 25,
        EaseOutCirc = 26,
        EaseInOutCirc = 27,
        EaseOutInCirc = 28,
        EaseInElastic = 29,
        EaseOutElastic = 30,
        EaseInOutElastic = 31,
        EaseOutInElastic = 32,
        EaseInBack = 33,
        EaseOutBack = 34,
        EaseInOutBack = 35,
        EaseOutInBack = 36,
        EaseInBounce = 37,
        EaseOutBounce = 38,
        EaseInOutBounce = 39,
        EaseOutInBounce = 40
    }

    public class Equations
    {


        // TWEENING EQUATIONS floats -----------------------------------------------------------------------------------------------------
        // (the original equations are Robert Penner's work as mentioned on the disclaimer)

        /**
         * Easing equation float for a simple linear tweening, with no easing.
         *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */

        public static float EaseNone(float t, float b, float c, float d)
        {
            return c * t / d + b;
        }

        /**
         * Easing equation float for a quadratic (t^2) easing in: accelerating from zero velocity.
         *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseInQuad(float t, float b, float c, float d)
        {
            return c * (t /= d) * t + b;
        }

        /**
         * Easing equation float for a quadratic (t^2) easing out: decelerating to zero velocity.
         *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseOutQuad(float t, float b, float c, float d)
        {
            return -c * (t /= d) * (t - 2) + b;
        }

        /**
         * Easing equation float for a quadratic (t^2) easing in/out: acceleration until halfway, then deceleration.
         *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseInOutQuad(float t, float b, float c, float d)
        {

            if ((t /= d / 2) < 1) return c / 2 * t * t + b;

            return -c / 2 * ((--t) * (t - 2) - 1) + b;
        }

        /**
         * Easing equation float for a quadratic (t^2) easing out/in: deceleration until halfway, then acceleration.
         *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseOutInQuad(float t, float b, float c, float d)
        {
            if (t < d / 2) return EaseOutQuad(t * 2, b, c / 2, d);
            return EaseInQuad((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
         * Easing equation float for a cubic (t^3) easing in: accelerating from zero velocity.
             *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseInCubic(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * t + b;
        }

        /**
         * Easing equation float for a cubic (t^3) easing out: decelerating from zero velocity.
             *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseOutCubic(float t, float b, float c, float d)
        {
            return c * ((t = t / d - 1) * t * t + 1) + b;
        }

        /**
         * Easing equation float for a cubic (t^3) easing in/out: acceleration until halfway, then deceleration.
             *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseInOutCubic(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t + 2) + b;
        }

        /**
         * Easing equation float for a cubic (t^3) easing out/in: deceleration until halfway, then acceleration.
             *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
        public static float EaseOutInCubic(float t, float b, float c, float d)
        {
            if (t < d / 2) return EaseOutCubic(t * 2, b, c / 2, d);
            return EaseInCubic((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
             * Easing equation float for a quartic (t^4) easing in: accelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseInQuart(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * t * t + b;
        }

        /**
             * Easing equation float for a quartic (t^4) easing out: decelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseOutQuart(float t, float b, float c, float d)
        {
            return -c * ((t = t / d - 1) * t * t * t - 1) + b;
        }

        /**
             * Easing equation float for a quartic (t^4) easing in/out: acceleration until halfway, then deceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseInOutQuart(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
            return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
        }

        /**
             * Easing equation float for a quartic (t^4) easing out/in: deceleration until halfway, then acceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseOutInQuart(float t, float b, float c, float d)
        {
            if (t < d / 2) return EaseOutQuart(t * 2, b, c / 2, d);
            return EaseInQuart((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
             * Easing equation float for a quintic (t^5) easing in: accelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseInQuint(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * t * t * t + b;
        }

        /**
             * Easing equation float for a quintic (t^5) easing out: decelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseOutQuint(float t, float b, float c, float d)
        {
            return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
        }

        /**
             * Easing equation float for a quintic (t^5) easing in/out: acceleration until halfway, then deceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseInOutQuint(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
        }

        /**
             * Easing equation float for a quintic (t^5) easing out/in: deceleration until halfway, then acceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseOutInQuint(float t, float b, float c, float d)
        {
            if (t < d / 2) return EaseOutQuint(t * 2, b, c / 2, d);
            return EaseInQuint((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
             * Easing equation float for a sinusoidal (sin(t)) easing in: accelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseInSine(float t, float b, float c, float d)
        {
            return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
        }

        /**
             * Easing equation float for a sinusoidal (sin(t)) easing out: decelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseOutSine(float t, float b, float c, float d)
        {
            return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
        }

        /**
             * Easing equation float for a sinusoidal (sin(t)) easing in/out: acceleration until halfway, then deceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseInOutSine(float t, float b, float c, float d)
        {
            return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
        }

        /**
             * Easing equation float for a sinusoidal (sin(t)) easing out/in: deceleration until halfway, then acceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseOutInSine(float t, float b, float c, float d)
        {
            if (t < d / 2) return EaseOutSine(t * 2, b, c / 2, d);
            return EaseInSine((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
             * Easing equation float for an exponential (2^t) easing in: accelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseInExpo(float t, float b, float c, float d)
        {
            return (t == 0) ? b : c * Mathf.Pow(2, 10 * (t / d - 1)) + b - c * 0.001f;
        }

        /**
             * Easing equation float for an exponential (2^t) easing out: decelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseOutExpo(float t, float b, float c, float d)
        {
            return (t == d) ? b + c : c * 1.001f * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
        }

        /**
             * Easing equation float for an exponential (2^t) easing in/out: acceleration until halfway, then deceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseInOutExpo(float t, float b, float c, float d)
        {
            if (t == 0) return b;
            if (t == d) return b + c;
            if ((t /= d / 2) < 1) return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b - c * 0.0005f;
            return c / 2 * 1.0005f * (-Mathf.Pow(2, -10 * --t) + 2) + b;
        }

        /**
             * Easing equation float for an exponential (2^t) easing out/in: deceleration until halfway, then acceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseOutInExpo(float t, float b, float c, float d)
        {
            if (t < d / 2) return EaseOutExpo(t * 2, b, c / 2, d);
            return EaseInExpo((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
             * Easing equation float for a circular (sqrt(1-t^2)) easing in: accelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseInCirc(float t, float b, float c, float d)
        {
            return -c * (Mathf.Sqrt(1 - (t /= d) * t) - 1) + b;
        }

        /**
             * Easing equation float for a circular (sqrt(1-t^2)) easing out: decelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseOutCirc(float t, float b, float c, float d)
        {
            return c * Mathf.Sqrt(1 - (t = t / d - 1) * t) + b;
        }

        /**
             * Easing equation float for a circular (sqrt(1-t^2)) easing in/out: acceleration until halfway, then deceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseInOutCirc(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1) return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
            return c / 2 * (Mathf.Sqrt(1 - (t -= 2) * t) + 1) + b;
        }

        /**
             * Easing equation float for a circular (sqrt(1-t^2)) easing out/in: deceleration until halfway, then acceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseOutInCirc(float t, float b, float c, float d)
        {
            if (t < d / 2) return EaseOutCirc(t * 2, b, c / 2, d);
            return EaseInCirc((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
             * Easing equation float for an elastic (exponentially decaying sine wave) easing in: accelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @param a		Amplitude.
             * @param p		Period.
             * @return		The correct value.
             */
        public static float EaseInElastic(float t, float b, float c, float d)
        {
            if (t == 0) return b;
            if ((t /= d) == 1) return b + c;
            float p = d * .3f;
            float s = 0;
            float a = 0;
            if (a == 0f || a < Mathf.Abs(c))
            {
                a = c;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(c / a);
            }
            return -(a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
        }

        /**
             * Easing equation float for an elastic (exponentially decaying sine wave) easing out: decelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @param a		Amplitude.
             * @param p		Period.
             * @return		The correct value.
             */
        public static float EaseOutElastic(float t, float b, float c, float d)
        {
            if (t == 0) return b;
            if ((t /= d) == 1) return b + c;
            float p = d * .3f;
            float s = 0;
            float a = 0;
            if (a == 0f || a < Mathf.Abs(c))
            {
                a = c;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(c / a);
            }
            return (a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + c + b);
        }

        /**
             * Easing equation float for an elastic (exponentially decaying sine wave) easing in/out: acceleration until halfway, then deceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @param a		Amplitude.
             * @param p		Period.
             * @return		The correct value.
             */
        public static float EaseInOutElastic(float t, float b, float c, float d)
        {
            if (t == 0) return b;
            if ((t /= d / 2) == 2) return b + c;
            float p = d * (.3f * 1.5f);
            float s = 0;
            float a = 0;
            if (a == 0f || a < Mathf.Abs(c))
            {
                a = c;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(c / a);
            }
            if (t < 1) return -.5f * (a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
            return a * Mathf.Pow(2, -10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) * .5f + c + b;
        }

        /**
             * Easing equation float for an elastic (exponentially decaying sine wave) easing out/in: deceleration until halfway, then acceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @param a		Amplitude.
             * @param p		Period.
             * @return		The correct value.
             */
        public static float EaseOutInElastic(float t, float b, float c, float d)
        {
            if (t < d / 2) return EaseOutElastic(t * 2, b, c / 2, d);
            return EaseInElastic((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
             * Easing equation float for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in: accelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @param s		Overshoot ammount: higher s means greater overshoot (0 produces cubic easing with no overshoot, and the default value of 1.70158 produces an overshoot of 10 percent).
             * @return		The correct value.
             */
        public static float EaseInBack(float t, float b, float c, float d)
        {
            float s = 1.70158f;
            return c * (t /= d) * t * ((s + 1) * t - s) + b;
        }

        /**
             * Easing equation float for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out: decelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @param s		Overshoot ammount: higher s means greater overshoot (0 produces cubic easing with no overshoot, and the default value of 1.70158 produces an overshoot of 10 percent).
             * @return		The correct value.
             */
        public static float EaseOutBack(float t, float b, float c, float d)
        {
            float s = 1.70158f;
            return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
        }

        /**
             * Easing equation float for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out: acceleration until halfway, then deceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @param s		Overshoot ammount: higher s means greater overshoot (0 produces cubic easing with no overshoot, and the default value of 1.70158 produces an overshoot of 10 percent).
             * @return		The correct value.
             */
        public static float EaseInOutBack(float t, float b, float c, float d)
        {
            float s = 1.70158f;
            if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
            return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
        }

        /**
             * Easing equation float for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in: deceleration until halfway, then acceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @param s		Overshoot ammount: higher s means greater overshoot (0 produces cubic easing with no overshoot, and the default value of 1.70158 produces an overshoot of 10 percent).
             * @return		The correct value.
             */
        public static float EaseOutInBack(float t, float b, float c, float d)
        {
            if (t < d / 2) return EaseOutBack(t * 2, b, c / 2, d);
            return EaseInBack((t * 2) - d, b + c / 2, c / 2, d);
        }

        /**
             * Easing equation float for a bounce (exponentially decaying parabolic bounce) easing in: accelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseInBounce(float t, float b, float c, float d)
        {
            return c - EaseOutBounce(d - t, 0, c, d) + b;
        }

        /**
             * Easing equation float for a bounce (exponentially decaying parabolic bounce) easing out: decelerating from zero velocity.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseOutBounce(float t, float b, float c, float d)
        {
            if ((t /= d) < (1 / 2.75f))
            {
                return c * (7.5625f * t * t) + b;
            }
            else if (t < (2 / 2.75f))
            {
                return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
            }
            else if (t < (2.5f / 2.75f))
            {
                return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
            }
            else
            {
                return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
            }
        }

        /**
             * Easing equation float for a bounce (exponentially decaying parabolic bounce) easing in/out: acceleration until halfway, then deceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseInOutBounce(float t, float b, float c, float d)
        {
            if (t < d / 2) return EaseInBounce(t * 2, 0, c, d) * .5f + b;
            else return EaseOutBounce(t * 2 - d, 0, c, d) * .5f + c * .5f + b;
        }

        /**
             * Easing equation float for a bounce (exponentially decaying parabolic bounce) easing out/in: deceleration until halfway, then acceleration.
             *
             * @param t		Current time (in frames or seconds).
             * @param b		Starting value.
             * @param c		Change needed in value.
             * @param d		Expected easing duration (in frames or seconds).
             * @return		The correct value.
             */
        public static float EaseOutInBounce(float t, float b, float c, float d)
        {
            if (t < d / 2) return EaseOutBounce(t * 2, b, c / 2, d);
            return EaseInBounce((t * 2) - d, b + c / 2, c / 2, d);
        }
        public delegate float EaseDelegate(float t, float b, float c, float d);

        public static EaseDelegate[] methods = new EaseDelegate[]{
		EaseNone,
		EaseInQuad,
		EaseOutQuad,
		EaseInOutQuad,
		EaseOutInQuad,
		EaseInCubic,
		EaseOutCubic,
		EaseInOutCubic,
		EaseOutInCubic,
		EaseInQuart, 
		EaseOutQuart,  
		EaseInOutQuart,  
		EaseOutInQuart,  
		EaseInQuint,  
		EaseOutQuint,  
		EaseInOutQuint,  
		EaseOutInQuint,
		EaseInSine, 
		EaseOutSine, 
		EaseInOutSine, 
		EaseOutInSine, 
		EaseInExpo,  
		EaseOutExpo,  
		EaseInOutExpo,  
		EaseOutInExpo,
		EaseInCirc,  
		EaseOutCirc,  
		EaseInOutCirc,  
		EaseOutInCirc,  
		EaseInElastic,  
		EaseOutElastic,  
		EaseInOutElastic,  
		EaseOutInElastic,
		EaseInBack, 
		EaseOutBack, 
		EaseInOutBack, 
		EaseOutInBack, 
		EaseInBounce, 
		EaseOutBounce, 
		EaseInOutBounce,  
		EaseOutInBounce
	    };
    }
}