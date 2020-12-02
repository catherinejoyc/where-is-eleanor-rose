/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_ATMOGRAVEYARD = 2080650348U;
        static const AkUniqueID PLAY_BTGLITCH = 172364415U;
        static const AkUniqueID PLAY_CLEARSIGHT = 1545484324U;
        static const AkUniqueID PLAY_CS_TRIGGER = 2192129965U;
        static const AkUniqueID PLAY_CSGLITCH = 4278918491U;
        static const AkUniqueID PLAY_DAISY = 4057232700U;
        static const AkUniqueID PLAY_GLITCHBT = 1324495071U;
        static const AkUniqueID PLAY_MINION_FOOTSTEPS = 1262694724U;
        static const AkUniqueID PLAY_MUSIC = 2932040671U;
        static const AkUniqueID PLAY_NAZAR = 953859796U;
        static const AkUniqueID PLAY_PLAYER_FOOTSTEPS = 98439365U;
        static const AkUniqueID PLAY_TEXTSOUND = 3625099708U;
        static const AkUniqueID STOP_ATMOGRAVEYARD = 4058000234U;
        static const AkUniqueID STOP_CLEARSIGHT = 1593485562U;
        static const AkUniqueID STOP_CSGLITCH = 1482370841U;
        static const AkUniqueID STOP_MINION_FOOTSTEPS = 3801597854U;
        static const AkUniqueID STOP_MUSIC = 2837384057U;
        static const AkUniqueID STOP_MUSIC_NOFADE = 650299671U;
        static const AkUniqueID STOP_PLAYER_FOOTSTEPS = 2284278951U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace MUSIC
        {
            static const AkUniqueID GROUP = 3991942870U;

            namespace STATE
            {
                static const AkUniqueID ANGERSTATE = 127485845U;
                static const AkUniqueID BARGAININGSTATE = 734953026U;
                static const AkUniqueID BT_ST = 885775863U;
                static const AkUniqueID BT_V2STATE = 3085453093U;
                static const AkUniqueID DENIALSTATE = 2602919295U;
                static const AkUniqueID DEPRESSIONSTATE = 860641656U;
            } // namespace STATE
        } // namespace MUSIC

    } // namespace STATES

    namespace SWITCHES
    {
        namespace ANGER_SG
        {
            static const AkUniqueID GROUP = 2515940707U;

            namespace SWITCH
            {
                static const AkUniqueID ANGER1 = 3629523371U;
                static const AkUniqueID ANGER2 = 3629523368U;
                static const AkUniqueID ANGER3 = 3629523369U;
            } // namespace SWITCH
        } // namespace ANGER_SG

        namespace BARGAINING_SG
        {
            static const AkUniqueID GROUP = 1094663664U;

            namespace SWITCH
            {
                static const AkUniqueID BARGAINING1 = 2727595076U;
                static const AkUniqueID BARGAINING2 = 2727595079U;
                static const AkUniqueID BARGAINING3 = 2727595078U;
            } // namespace SWITCH
        } // namespace BARGAINING_SG

        namespace DENIAL_SG
        {
            static const AkUniqueID GROUP = 659192053U;

            namespace SWITCH
            {
                static const AkUniqueID DENIAL1 = 3810638041U;
                static const AkUniqueID DENIAL2 = 3810638042U;
                static const AkUniqueID DENIAL3 = 3810638043U;
            } // namespace SWITCH
        } // namespace DENIAL_SG

        namespace DEPRESSION_SG
        {
            static const AkUniqueID GROUP = 3747756326U;

            namespace SWITCH
            {
                static const AkUniqueID DEPRESSION1 = 3362116182U;
                static const AkUniqueID DEPRESSION2 = 3362116181U;
                static const AkUniqueID DEPRESSION3 = 3362116180U;
            } // namespace SWITCH
        } // namespace DEPRESSION_SG

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID LOWPASS_RTPC = 1119582304U;
        static const AkUniqueID PLAYBACK_RATE = 1524500807U;
        static const AkUniqueID PLAYERLOCATION_RTPC = 332625859U;
        static const AkUniqueID RPM = 796049864U;
        static const AkUniqueID SS_AIR_FEAR = 1351367891U;
        static const AkUniqueID SS_AIR_FREEFALL = 3002758120U;
        static const AkUniqueID SS_AIR_FURY = 1029930033U;
        static const AkUniqueID SS_AIR_MONTH = 2648548617U;
        static const AkUniqueID SS_AIR_PRESENCE = 3847924954U;
        static const AkUniqueID SS_AIR_RPM = 822163944U;
        static const AkUniqueID SS_AIR_SIZE = 3074696722U;
        static const AkUniqueID SS_AIR_STORM = 3715662592U;
        static const AkUniqueID SS_AIR_TIMEOFDAY = 3203397129U;
        static const AkUniqueID SS_AIR_TURBULENCE = 4160247818U;
    } // namespace GAME_PARAMETERS

    namespace TRIGGERS
    {
        static const AkUniqueID CLEARSIGHT_TRIGGER = 180550934U;
    } // namespace TRIGGERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID SOUNDBANK1 = 1647770721U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MOTION_FACTORY_BUS = 985987111U;
        static const AkUniqueID MUSIC_BUS = 3127962312U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID DEFAULT_MOTION_DEVICE = 4230635974U;
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
