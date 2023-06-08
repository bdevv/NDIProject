using System;
using System.Collections.Generic;
using System.Text;

namespace CAPINDIStreaming
{
    
    struct Position3d
    {
        public double x;
        public double y;
        public double z;
    }

    /* QUATERNION Structure */
    struct QuatRotation
    {
        public double q0;
        public double qx;
        public double qy;
        public double qz;
    }
    struct QuatTransformation
    {

        public QuatRotation rotation;
        public Position3d translation;
    }
    class Conversion
    {
        /* math numbers */
        const double BAD_double = (double)(-3.697314E28);
        const double MAX_NEGATIVE = (double)-3.0E28;
        public static QuatTransformation QuatInverseXfrm(QuatTransformation pdtXfrm)
        {
            QuatTransformation pdtNewXfrm = new QuatTransformation();
            pdtNewXfrm.rotation.q0 = pdtXfrm.rotation.q0;
            pdtNewXfrm.rotation.qx = -pdtXfrm.rotation.qx;
            pdtNewXfrm.rotation.qy = -pdtXfrm.rotation.qy;
            pdtNewXfrm.rotation.qz = -pdtXfrm.rotation.qz;
            pdtNewXfrm.translation = QuatRotatePoint(pdtNewXfrm.rotation, pdtXfrm.translation);

            pdtNewXfrm.translation.x = -pdtNewXfrm.translation.x;
            pdtNewXfrm.translation.y = -pdtNewXfrm.translation.y;
            pdtNewXfrm.translation.z = -pdtNewXfrm.translation.z;
            return pdtNewXfrm;
        }
        public static QuatTransformation QuatCombineXfrms(QuatTransformation pdtXfrm12, QuatTransformation pdtXfrm23)
        {
            QuatTransformation pdtXfrm13 = new QuatTransformation();
            QuatRotation  pdtQ12 = pdtXfrm12.rotation,pdtQ23 = pdtXfrm23.rotation;
            double fA,fB,fC,fD,fE,fF,fG,fH;

            fA = (pdtQ23.q0 + pdtQ23.qx) * (pdtQ12.q0 + pdtQ12.qx);
            fB = (pdtQ23.qz - pdtQ23.qy) * (pdtQ12.qy - pdtQ12.qz);
            fC = (pdtQ23.qx - pdtQ23.q0) * (pdtQ12.qy + pdtQ12.qz);
            fD = (pdtQ23.qy + pdtQ23.qz) * (pdtQ12.qx - pdtQ12.q0);
            fE = (pdtQ23.qx + pdtQ23.qz) * (pdtQ12.qx + pdtQ12.qy);
            fF = (pdtQ23.qx - pdtQ23.qz) * (pdtQ12.qx - pdtQ12.qy);
            fG = (pdtQ23.q0 + pdtQ23.qy) * (pdtQ12.q0 - pdtQ12.qz);
            fH = (pdtQ23.q0 - pdtQ23.qy) * (pdtQ12.q0 + pdtQ12.qz);

            pdtXfrm13.rotation.q0 = (double)(fB + (-fE - fF + fG + fH) / 2.0);
            pdtXfrm13.rotation.qx = (double)(fA - (fE + fF + fG + fH) / 2.0);
            pdtXfrm13.rotation.qy = (double)(-fC + (fE - fF + fG - fH) / 2.0);
            pdtXfrm13.rotation.qz = (double)(-fD + (fE - fF - fG + fH) / 2.0);

            pdtXfrm13.translation = QuatRotatePoint(pdtXfrm23.rotation, pdtXfrm12.translation);
            pdtXfrm13.translation.x += pdtXfrm23.translation.x;
            pdtXfrm13.translation.y += pdtXfrm23.translation.y;
            pdtXfrm13.translation.z += pdtXfrm23.translation.z;
            return pdtXfrm13;
        } /* QuatCombineXfrms */
        public static Position3d QuatRotatePoint(QuatRotation RotationQuaternionPtr,Position3d OriginalPositionPtr)
        {
            Position3d RotatedPositionPtr = new Position3d();
            Position3d UCrossV = new Position3d();      

            if (OriginalPositionPtr.x < MAX_NEGATIVE ||
                OriginalPositionPtr.y < MAX_NEGATIVE ||
                OriginalPositionPtr.z < MAX_NEGATIVE)
            {
                RotatedPositionPtr.x = RotatedPositionPtr.y = RotatedPositionPtr.z = BAD_double;

                return RotatedPositionPtr;
            } /* if */

            UCrossV.x = RotationQuaternionPtr.qy * OriginalPositionPtr.z
                        - RotationQuaternionPtr.qz * OriginalPositionPtr.y;
            UCrossV.y = RotationQuaternionPtr.qz * OriginalPositionPtr.x
                        - RotationQuaternionPtr.qx * OriginalPositionPtr.z;
            UCrossV.z = RotationQuaternionPtr.qx * OriginalPositionPtr.y
                        - RotationQuaternionPtr.qy * OriginalPositionPtr.x;

            RotatedPositionPtr.x = (double)(OriginalPositionPtr.x
                                        + 2.0 * (RotationQuaternionPtr.q0 * UCrossV.x
                                                + RotationQuaternionPtr.qy * UCrossV.z
                                                - RotationQuaternionPtr.qz * UCrossV.y));
            RotatedPositionPtr.y = (double)(OriginalPositionPtr.y
                                        + 2.0 * (RotationQuaternionPtr.q0 * UCrossV.y
                                                + RotationQuaternionPtr.qz * UCrossV.x
                                                - RotationQuaternionPtr.qx * UCrossV.z));
            RotatedPositionPtr.z = (double)(OriginalPositionPtr.z
                                        + 2.0 * (RotationQuaternionPtr.q0 * UCrossV.z
                                                + RotationQuaternionPtr.qx * UCrossV.y
                                                - RotationQuaternionPtr.qy * UCrossV.x));
            return RotatedPositionPtr;
        }
    }
}
