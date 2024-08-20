namespace Mathlib;

public static class bumpvects
{
    public const float OO_SQRT_2 = 0.70710676908493042f;
    public const float OO_SQRT_3 = 0.57735025882720947f;
    public const float OO_SQRT_6 = 0.40824821591377258f;

    public const float OO_SQRT_2_OVER_3 = 0.81649661064147949f;

    public const int NUM_BUMP_VECTS = 3;

    public static TableVector[] g_localBumpBasis =
    {
        new TableVector { x =  OO_SQRT_2_OVER_3, y = 0.0f      , z = OO_SQRT_3 },
        new TableVector { x = -OO_SQRT_6       , y = OO_SQRT_2 , z = OO_SQRT_3 },
        new TableVector { x = -OO_SQRT_6       , y = -OO_SQRT_2, z = OO_SQRT_3 },
    };

    public static void GetBumpNormals(Vector sVect, Vector tVect, Vector flatNormal,
                                      Vector phongNormal, Vector[] bumpNormals)
    {

    }
}