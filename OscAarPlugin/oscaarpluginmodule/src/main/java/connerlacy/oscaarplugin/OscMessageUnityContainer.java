package connerlacy.oscaarplugin;

import com.illposed.osc.OSCMessage;
import java.util.List;

/**
 * Created by connerlacy on 10/5/17.
 */

public class OscMessageUnityContainer
{
    public static List<OSCMessage> oscMessages;

    public static int getNumMessages()
    {
        return oscMessages.size();
    }

    public static float getMessageFloatArg(int indx)
    {
        if(oscMessages.size() < indx)
        {
            if(oscMessages.get(i).getArguments().size())
            {
                return (Float) oscMessages.get(indx).getArguments().get(0);
            }
        }

        return -1.0f;
    }

    public static String getMessageAddress(int indx)
    {
        if(oscMessages.size() < indx)
        {
            return oscMessages.get(indx).getAddress();
        }

        return "";
    }
}
