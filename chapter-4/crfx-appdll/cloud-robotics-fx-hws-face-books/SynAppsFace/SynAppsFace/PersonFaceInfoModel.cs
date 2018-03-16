using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using HwsFaceCores;

namespace SynAppsFace
{
    public class PersonFaceInfoModel
    {
        static private SqlConnectionStringBuilder SqlBuilder = new SqlConnectionStringBuilder();

        public long Id { set; get; }
        public string PersonId { set; get; }
        public string FaceConfidence { set; get; }
        public double FaceRectangle_Width { set; get; }
        public double FaceRectangle_Height { set; get; }
        public double FaceRectangle_Left { set; get; }
        public double FaceRectangle_Top { set; get; }
        public double FaceLandmarks_PupilLeft_X { set; get; }
        public double FaceLandmarks_PupilLeft_Y { set; get; }
        public double FaceLandmarks_PupilRight_X { set; get; }
        public double FaceLandmarks_PupilRight_Y { set; get; }
        public double FaceLandmarks_NoseTip_X { set; get; }
        public double FaceLandmarks_NoseTip_Y { set; get; }
        public double FaceLandmarks_MouseLeft_X { set; get; }
        public double FaceLandmarks_MouseLeft_Y { set; get; }
        public double FaceLandmarks_MouseRight_X { set; get; }
        public double FaceLandmarks_MouseRight_Y { set; get; }
        public double FaceLandmarks_EyebrowLeftOuter_X { set; get; }
        public double FaceLandmarks_EyebrowLeftOuter_Y { set; get; }
        public double FaceLandmarks_EyebrowLeftInner_X { set; get; }
        public double FaceLandmarks_EyebrowLeftInner_Y { set; get; }
        public double FaceLandmarks_EyeLeftOuter_X { set; get; }
        public double FaceLandmarks_EyeLeftOuter_Y { set; get; }
        public double FaceLandmarks_EyeLeftTop_X { set; get; }
        public double FaceLandmarks_EyeLeftTop_Y { set; get; }
        public double FaceLandmarks_EyeLeftBottom_X { set; get; }
        public double FaceLandmarks_EyeLeftBottom_Y { set; get; }
        public double FaceLandmarks_EyeLeftInner_X { set; get; }
        public double FaceLandmarks_EyeLeftInner_Y { set; get; }
        public double FaceLandmarks_EyebrowRightInner_X { set; get; }
        public double FaceLandmarks_EyebrowRightInner_Y { set; get; }
        public double FaceLandmarks_EyebrowRightOuter_X { set; get; }
        public double FaceLandmarks_EyebrowRightOuter_Y { set; get; }
        public double FaceLandmarks_EyeRightInner_X { set; get; }
        public double FaceLandmarks_EyeRightInner_Y { set; get; }
        public double FaceLandmarks_EyeRightTop_X { set; get; }
        public double FaceLandmarks_EyeRightTop_Y { set; get; }
        public double FaceLandmarks_EyeRightBottom_X { set; get; }
        public double FaceLandmarks_EyeRightBottom_Y { set; get; }
        public double FaceLandmarks_EyeRightOuter_X { set; get; }
        public double FaceLandmarks_EyeRightOuter_Y { set; get; }
        public double FaceLandmarks_NoseRootLeft_X { set; get; }
        public double FaceLandmarks_NoseRootLeft_Y { set; get; }
        public double FaceLandmarks_NoseRootRight_X { set; get; }
        public double FaceLandmarks_NoseRootRight_Y { set; get; }
        public double FaceLandmarks_NoseLeftAlarTop_X { set; get; }
        public double FaceLandmarks_NoseLeftAlarTop_Y { set; get; }
        public double FaceLandmarks_NoseRightAlarTop_X { set; get; }
        public double FaceLandmarks_NoseRightAlarTop_Y { set; get; }
        public double FaceLandmarks_NoseLeftAlarOutTip_X { set; get; }
        public double FaceLandmarks_NoseLeftAlarOutTip_Y { set; get; }
        public double FaceLandmarks_NoseRightAlarOutTip_X { set; get; }
        public double FaceLandmarks_NoseRightAlarOutTip_Y { set; get; }
        public double FaceLandmarks_UpperLipTop_X { set; get; }
        public double FaceLandmarks_UpperLipTop_Y { set; get; }
        public double FaceLandmarks_UpperLipBottom_X { set; get; }
        public double FaceLandmarks_UpperLipBottom_Y { set; get; }
        public double FaceLandmarks_UnderLipTop_X { set; get; }
        public double FaceLandmarks_UnderLipTop_Y { set; get; }
        public double FaceLandmarks_UnderLipBottom_X { set; get; }
        public double FaceLandmarks_UnderLipBottom_Y { set; get; }
        public double FaceAttributes_Age { set; get; }
        public string FaceAttributes_Gender { set; get; }
        public double FaceAttributes_Smile { set; get; }
        public double FaceAttributes_FacialHair_Moustache { set; get; }
        public double FaceAttributes_FacialHair_Beard { set; get; }
        public double FaceAttributes_FacialHair_Sideburns { set; get; }
        public string FaceAttributes_Glasses { set; get; }
        public double FaceAttributes_HeadPose_Roll { set; get; }
        public double FaceAttributes_HeadPose_Yaw { set; get; }
        public double FaceAttributes_HeadPose_Pitch { set; get; }
        public bool IsDeleted { set; get; }

        PersonFaceInfoModel()
        {
            this.Id = 0;
            this.IsDeleted = false;
        }

        public static void Connection(string _connectionString)
        {
            SqlBuilder.ConnectionString = _connectionString;
        }

        public static PersonFaceInfoModel New()
        {
            return new PersonFaceInfoModel();
        }

        public void Save()
        {
            var dc = new PersonFaceInfoDataContext(SqlBuilder.ConnectionString);
            if (this.Id == 0)
            {
                dc.PersonFaceInfos.InsertOnSubmit(new PersonFaceInfo
                {
                    PersonId                            = this.PersonId,
                    FaceConfidence                      = this.FaceConfidence,
                    FaceRectangle_Width                 = this.FaceRectangle_Width,
                    FaceRectangle_Height                = this.FaceRectangle_Height,
                    FaceRectangle_Left                  = this.FaceRectangle_Left,
                    FaceRectangle_Top                   = this.FaceRectangle_Top,
                    FaceLandmarks_PupilLeft_X           = this.FaceLandmarks_PupilLeft_X,
                    FaceLandmarks_PupilLeft_Y           = this.FaceLandmarks_PupilLeft_Y,
                    FaceLandmarks_PupilRight_X          = this.FaceLandmarks_PupilRight_X,
                    FaceLandmarks_PupilRight_Y          = this.FaceLandmarks_PupilRight_Y,
                    FaceLandmarks_NoseTip_X             = this.FaceLandmarks_NoseTip_X,
                    FaceLandmarks_NoseTip_Y             = this.FaceLandmarks_NoseTip_Y,
                    FaceLandmarks_MouseLeft_X           = this.FaceLandmarks_MouseLeft_X,
                    FaceLandmarks_MouseLeft_Y           = this.FaceLandmarks_MouseLeft_Y,
                    FaceLandmarks_MouseRight_X          = this.FaceLandmarks_MouseRight_X,
                    FaceLandmarks_MouseRight_Y          = this.FaceLandmarks_MouseRight_Y,
                    FaceLandmarks_EyebrowLeftOuter_X    = this.FaceLandmarks_EyebrowLeftOuter_X,
                    FaceLandmarks_EyebrowLeftOuter_Y    = this.FaceLandmarks_EyebrowLeftOuter_Y,
                    FaceLandmarks_EyebrowLeftInner_X    = this.FaceLandmarks_EyebrowLeftInner_X,
                    FaceLandmarks_EyebrowLeftInner_Y    = this.FaceLandmarks_EyebrowLeftInner_Y,
                    FaceLandmarks_EyeLeftOuter_X        = this.FaceLandmarks_EyeLeftOuter_X,
                    FaceLandmarks_EyeLeftOuter_Y        = this.FaceLandmarks_EyeLeftOuter_Y,
                    FaceLandmarks_EyeLeftTop_X          = this.FaceLandmarks_EyeLeftTop_X,
                    FaceLandmarks_EyeLeftTop_Y          = this.FaceLandmarks_EyeLeftTop_Y,
                    FaceLandmarks_EyeLeftBottom_X       = this.FaceLandmarks_EyeLeftBottom_X,
                    FaceLandmarks_EyeLeftBottom_Y       = this.FaceLandmarks_EyeLeftBottom_Y,
                    FaceLandmarks_EyeLeftInner_X        = this.FaceLandmarks_EyeLeftInner_X,
                    FaceLandmarks_EyeLeftInner_Y        = this.FaceLandmarks_EyeLeftInner_Y,
                    FaceLandmarks_EyebrowRightInner_X   = this.FaceLandmarks_EyebrowRightInner_X,
                    FaceLandmarks_EyebrowRightInner_Y   = this.FaceLandmarks_EyebrowRightInner_Y,
                    FaceLandmarks_EyebrowRightOuter_X   = this.FaceLandmarks_EyebrowRightOuter_X,
                    FaceLandmarks_EyebrowRightOuter_Y   = this.FaceLandmarks_EyebrowRightOuter_Y,
                    FaceLandmarks_EyeRightInner_X       = this.FaceLandmarks_EyeRightInner_X,
                    FaceLandmarks_EyeRightInner_Y       = this.FaceLandmarks_EyeRightInner_Y,
                    FaceLandmarks_EyeRightTop_X         = this.FaceLandmarks_EyeRightTop_X,
                    FaceLandmarks_EyeRightTop_Y         = this.FaceLandmarks_EyeRightTop_Y,
                    FaceLandmarks_EyeRightBottom_X      = this.FaceLandmarks_EyeRightBottom_X,
                    FaceLandmarks_EyeRightBottom_Y      = this.FaceLandmarks_EyeRightBottom_Y,
                    FaceLandmarks_EyeRightOuter_X       = this.FaceLandmarks_EyeRightOuter_X,
                    FaceLandmarks_EyeRightOuter_Y       = this.FaceLandmarks_EyeRightOuter_Y,
                    FaceLandmarks_NoseRootLeft_X        = this.FaceLandmarks_NoseRootLeft_X,
                    FaceLandmarks_NoseRootLeft_Y        = this.FaceLandmarks_NoseRootLeft_Y,
                    FaceLandmarks_NoseRootRight_X       = this.FaceLandmarks_NoseRootRight_X,
                    FaceLandmarks_NoseRootRight_Y       = this.FaceLandmarks_NoseRootRight_Y,
                    FaceLandmarks_NoseLeftAlarTop_X     = this.FaceLandmarks_NoseLeftAlarTop_X,
                    FaceLandmarks_NoseLeftAlarTop_Y     = this.FaceLandmarks_NoseLeftAlarTop_Y,
                    FaceLandmarks_NoseRightAlarTop_X    = this.FaceLandmarks_NoseRightAlarTop_X,
                    FaceLandmarks_NoseRightAlarTop_Y    = this.FaceLandmarks_NoseRightAlarTop_Y,
                    FaceLandmarks_NoseLeftAlarOutTip_X  = this.FaceLandmarks_NoseLeftAlarOutTip_X,
                    FaceLandmarks_NoseLeftAlarOutTip_Y  = this.FaceLandmarks_NoseLeftAlarOutTip_Y,
                    FaceLandmarks_NoseRightAlarOutTip_X = this.FaceLandmarks_NoseRightAlarOutTip_X,
                    FaceLandmarks_NoseRightAlarOutTip_Y = this.FaceLandmarks_NoseRightAlarOutTip_Y,
                    FaceLandmarks_UpperLipTop_X         = this.FaceLandmarks_UpperLipTop_X,
                    FaceLandmarks_UpperLipTop_Y         = this.FaceLandmarks_UpperLipTop_Y,
                    FaceLandmarks_UpperLipBottom_X      = this.FaceLandmarks_UpperLipBottom_X,
                    FaceLandmarks_UpperLipBottom_Y      = this.FaceLandmarks_UpperLipBottom_Y,
                    FaceLandmarks_UnderLipTop_X         = this.FaceLandmarks_UnderLipTop_X,
                    FaceLandmarks_UnderLipTop_Y         = this.FaceLandmarks_UnderLipTop_Y,
                    FaceLandmarks_UnderLipBottom_X      = this.FaceLandmarks_UnderLipBottom_X,
                    FaceLandmarks_UnderLipBottom_Y      = this.FaceLandmarks_UnderLipBottom_Y,
                    FaceAttributes_Age                  = this.FaceAttributes_Age,
                    FaceAttributes_Gender               = this.FaceAttributes_Gender,
                    FaceAttributes_Smile                = this.FaceAttributes_Smile,
                    FaceAttributes_FacialHair_Moustache = this.FaceAttributes_FacialHair_Moustache,
                    FaceAttributes_FacialHair_Beard     = this.FaceAttributes_FacialHair_Beard,
                    FaceAttributes_FacialHair_Sideburns = this.FaceAttributes_FacialHair_Sideburns,
                    FaceAttributes_Glasses              = this.FaceAttributes_Glasses,
                    FaceAttributes_HeadPose_Roll        = this.FaceAttributes_HeadPose_Roll,
                    FaceAttributes_HeadPose_Yaw         = this.FaceAttributes_HeadPose_Yaw,
                    FaceAttributes_HeadPose_Pitch       = this.FaceAttributes_HeadPose_Pitch,
                    CreatedAt                           = DateTime.Now
                });
            }
            else
            {
                var records =
                    from n in dc.PersonFaceInfos
                    where n.Id == this.Id
                    select n;

                foreach (var r in records)
                {
                    r.PersonId = this.PersonId;
                    r.FaceConfidence = this.FaceConfidence;
                    r.FaceRectangle_Width = this.FaceRectangle_Width;
                    r.FaceRectangle_Height = this.FaceRectangle_Height;
                    r.FaceRectangle_Left = this.FaceRectangle_Left;
                    r.FaceRectangle_Top = this.FaceRectangle_Top;
                    r.FaceLandmarks_PupilLeft_X = this.FaceLandmarks_PupilLeft_X;
                    r.FaceLandmarks_PupilLeft_Y = this.FaceLandmarks_PupilLeft_Y;
                    r.FaceLandmarks_PupilRight_X = this.FaceLandmarks_PupilRight_X;
                    r.FaceLandmarks_PupilRight_Y = this.FaceLandmarks_PupilRight_Y;
                    r.FaceLandmarks_NoseTip_X = this.FaceLandmarks_NoseTip_X;
                    r.FaceLandmarks_NoseTip_Y = this.FaceLandmarks_NoseTip_Y;
                    r.FaceLandmarks_MouseLeft_X = this.FaceLandmarks_MouseLeft_X;
                    r.FaceLandmarks_MouseLeft_Y = this.FaceLandmarks_MouseLeft_Y;
                    r.FaceLandmarks_MouseRight_X = this.FaceLandmarks_MouseRight_X;
                    r.FaceLandmarks_MouseRight_Y = this.FaceLandmarks_MouseRight_Y;
                    r.FaceLandmarks_EyebrowLeftOuter_X = this.FaceLandmarks_EyebrowLeftOuter_X;
                    r.FaceLandmarks_EyebrowLeftOuter_Y = this.FaceLandmarks_EyebrowLeftOuter_Y;
                    r.FaceLandmarks_EyebrowLeftInner_X = this.FaceLandmarks_EyebrowLeftInner_X;
                    r.FaceLandmarks_EyebrowLeftInner_Y = this.FaceLandmarks_EyebrowLeftInner_Y;
                    r.FaceLandmarks_EyeLeftOuter_X = this.FaceLandmarks_EyeLeftOuter_X;
                    r.FaceLandmarks_EyeLeftOuter_Y = this.FaceLandmarks_EyeLeftOuter_Y;
                    r.FaceLandmarks_EyeLeftTop_X = this.FaceLandmarks_EyeLeftTop_X;
                    r.FaceLandmarks_EyeLeftTop_Y = this.FaceLandmarks_EyeLeftTop_Y;
                    r.FaceLandmarks_EyeLeftBottom_X = this.FaceLandmarks_EyeLeftBottom_X;
                    r.FaceLandmarks_EyeLeftBottom_Y = this.FaceLandmarks_EyeLeftBottom_Y;
                    r.FaceLandmarks_EyeLeftInner_X = this.FaceLandmarks_EyeLeftInner_X;
                    r.FaceLandmarks_EyeLeftInner_Y = this.FaceLandmarks_EyeLeftInner_Y;
                    r.FaceLandmarks_EyebrowRightInner_X = this.FaceLandmarks_EyebrowRightInner_X;
                    r.FaceLandmarks_EyebrowRightInner_Y = this.FaceLandmarks_EyebrowRightInner_Y;
                    r.FaceLandmarks_EyebrowRightOuter_X = this.FaceLandmarks_EyebrowRightOuter_X;
                    r.FaceLandmarks_EyebrowRightOuter_Y = this.FaceLandmarks_EyebrowRightOuter_Y;
                    r.FaceLandmarks_EyeRightInner_X = this.FaceLandmarks_EyeRightInner_X;
                    r.FaceLandmarks_EyeRightInner_Y = this.FaceLandmarks_EyeRightInner_Y;
                    r.FaceLandmarks_EyeRightTop_X = this.FaceLandmarks_EyeRightTop_X;
                    r.FaceLandmarks_EyeRightTop_Y = this.FaceLandmarks_EyeRightTop_Y;
                    r.FaceLandmarks_EyeRightBottom_X = this.FaceLandmarks_EyeRightBottom_X;
                    r.FaceLandmarks_EyeRightBottom_Y = this.FaceLandmarks_EyeRightBottom_Y;
                    r.FaceLandmarks_EyeRightOuter_X = this.FaceLandmarks_EyeRightOuter_X;
                    r.FaceLandmarks_EyeRightOuter_Y = this.FaceLandmarks_EyeRightOuter_Y;
                    r.FaceLandmarks_NoseRootLeft_X = this.FaceLandmarks_NoseRootLeft_X;
                    r.FaceLandmarks_NoseRootLeft_Y = this.FaceLandmarks_NoseRootLeft_Y;
                    r.FaceLandmarks_NoseRootRight_X = this.FaceLandmarks_NoseRootRight_X;
                    r.FaceLandmarks_NoseRootRight_Y = this.FaceLandmarks_NoseRootRight_Y;
                    r.FaceLandmarks_NoseLeftAlarTop_X = this.FaceLandmarks_NoseLeftAlarTop_X;
                    r.FaceLandmarks_NoseLeftAlarTop_Y = this.FaceLandmarks_NoseLeftAlarTop_Y;
                    r.FaceLandmarks_NoseRightAlarTop_X = this.FaceLandmarks_NoseRightAlarTop_X;
                    r.FaceLandmarks_NoseRightAlarTop_Y = this.FaceLandmarks_NoseRightAlarTop_Y;
                    r.FaceLandmarks_NoseLeftAlarOutTip_X = this.FaceLandmarks_NoseLeftAlarOutTip_X;
                    r.FaceLandmarks_NoseLeftAlarOutTip_Y = this.FaceLandmarks_NoseLeftAlarOutTip_Y;
                    r.FaceLandmarks_NoseRightAlarOutTip_X = this.FaceLandmarks_NoseRightAlarOutTip_X;
                    r.FaceLandmarks_NoseRightAlarOutTip_Y = this.FaceLandmarks_NoseRightAlarOutTip_Y;
                    r.FaceLandmarks_UpperLipTop_X = this.FaceLandmarks_UpperLipTop_X;
                    r.FaceLandmarks_UpperLipTop_Y = this.FaceLandmarks_UpperLipTop_Y;
                    r.FaceLandmarks_UpperLipBottom_X = this.FaceLandmarks_UpperLipBottom_X;
                    r.FaceLandmarks_UpperLipBottom_Y = this.FaceLandmarks_UpperLipBottom_Y;
                    r.FaceLandmarks_UnderLipTop_X = this.FaceLandmarks_UnderLipTop_X;
                    r.FaceLandmarks_UnderLipTop_Y = this.FaceLandmarks_UnderLipTop_Y;
                    r.FaceLandmarks_UnderLipBottom_X = this.FaceLandmarks_UnderLipBottom_X;
                    r.FaceLandmarks_UnderLipBottom_Y = this.FaceLandmarks_UnderLipBottom_Y;
                    r.FaceAttributes_Age = this.FaceAttributes_Age;
                    r.FaceAttributes_Gender = this.FaceAttributes_Gender;
                    r.FaceAttributes_Smile = this.FaceAttributes_Smile;
                    r.FaceAttributes_FacialHair_Moustache = this.FaceAttributes_FacialHair_Moustache;
                    r.FaceAttributes_FacialHair_Beard = this.FaceAttributes_FacialHair_Beard;
                    r.FaceAttributes_FacialHair_Sideburns = this.FaceAttributes_FacialHair_Sideburns;
                    r.FaceAttributes_Glasses = this.FaceAttributes_Glasses;
                    r.FaceAttributes_HeadPose_Roll = this.FaceAttributes_HeadPose_Roll;
                    r.FaceAttributes_HeadPose_Yaw = this.FaceAttributes_HeadPose_Yaw;
                    r.FaceAttributes_HeadPose_Pitch = this.FaceAttributes_HeadPose_Pitch;
                    r.UpdatedAt = DateTime.Now;
                }
            }
            dc.SubmitChanges();
        }
    }
}
