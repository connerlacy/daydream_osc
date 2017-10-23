package connerlacy.oscaarplugin;

import android.support.annotation.NonNull;

import com.illposed.osc.OSCMessage;

import java.util.Collection;
import java.util.Iterator;
import java.util.ArrayList;
import java.util.ListIterator;

/**
 * Created by connerlacy on 10/5/17.
 */

public class OscMessageUnityContainer
{
    public static ArrayList<OSCMessage> oscMessages = new ArrayList<OSCMessage>();

    public static int getNumMessages()
    {
        return oscMessages.size();
    }

    public static float getMessageFloatArg(int indx)
    {
        // If there are any messages at this index
        if(oscMessages.size() > indx)
        {
            // And that message has some args
            if(oscMessages.get(indx).getArguments().size() > 0)
            {
                // Return the first argument (discard other should they be present for now)
                return (Float) oscMessages.get(indx).getArguments().get(0);
            }
        }

        return -1.0f;
    }

    public static String getMessageAddress(int indx)
    {
        if(oscMessages.size() > indx)
        {
            return oscMessages.get(indx).getAddress();
        }

        return "";
    }
}
