package connerlacy.oscaarplugin;

import android.util.Log;

import com.illposed.osc.*;
import java.net.SocketException;
import java.net.*;
import java.util.*;

import static connerlacy.oscaarplugin.OscMessageUnityContainer.oscMessages;


/**
 * Created by connerlacy on 9/29/17.
 */

public class OscAarPlugin
{
    private static String     ip         = "192.168.1.175";
    private static int        outputPort = 7777;
    private static OSCPortOut oscPortOut;

    public static  OscMessageUnityContainer omuc;

    private static Thread oscOutputThread = new Thread() {
        @Override
        public void run() {

            Log.d("DEBUG", "Starting OSC Output thread...");

            try {
                // Connect to some IP address and port
                Log.d("DEBUG", "Creating OSC output port");
                oscPortOut = new OSCPortOut(InetAddress.getByName(ip), outputPort);

            } catch(UnknownHostException e) {
                // Error handling when your IP isn't found
                Log.d("DEBUG IP", e.toString());
                return;
            } catch(Exception e) {
                // Error handling for any other errors
                Log.d("DEBUG",e.toString());
                return;
            }


      /* The second part of the run() method loops infinitely and sends messages every 500
       * milliseconds.
       */
            while (true) {
                if (oscPortOut != null) {
                    //Log.d("DEBUG", "port not null");
                    // Creating the message
                    Object[] thingsToSend = new Object[3];
                    thingsToSend[0] = "Hello World";
                    thingsToSend[1] = 12345;
                    thingsToSend[2] = 1.2345;

          /* The version of JavaOSC from the Maven Repository is slightly different from the one
           * from the download link on the main website at the time of writing this tutorial.
           *
           * The Maven Repository version (used here), takes a Collection, which is why we need
           * Arrays.asList(thingsToSend).
           *
           * If you're using the downloadable version for some reason, you should switch the
           * commented and uncommented lines for message below
           */
                    OSCMessage message = new OSCMessage("/Test1", Arrays.asList(thingsToSend));
                    // OSCMessage message = new OSCMessage(myIP, thingsToSend);


          /* NOTE: Since this version of JavaOSC uses Collections, we can actually use ArrayLists,
           * or any other class that implements the Collection interface. The following code is
           * valid for this version.
           *
           * The benefit of using an ArrayList is that you don't have to know how much information
           * you are sending ahead of time. You can add things to the end of an ArrayList, but not
           * to an Array.
           *
           * If you want to use this code with the downloadable version, you should switch the
           * commented and uncommented lines for message2
           */
                    ArrayList<Object> moreThingsToSend = new ArrayList<Object>();
                    moreThingsToSend.add("Hello World2");
                    moreThingsToSend.add(123456);
                    moreThingsToSend.add(12.345);

                    OSCMessage message2 = new OSCMessage("/Test2", moreThingsToSend);
                    //OSCMessage message2 = new OSCMessage(myIP, moreThingsToSend.toArray());

                    try {
                        // Send the messages
                        oscPortOut.send(message);
                        oscPortOut.send(message2);

                        //Log.d("DEBUG", "output");

                        // Pause for half a second
                        sleep(500);
                    } catch (Exception e) {
                        // Error handling for some error
                        Log.d("Sending Messages: ", e.toString());
                    }
                }
            }
        }
    };

    public static void startOSC()
    {
        oscOutputThread.start();

        try
        {
            OSCPortIn receiver = new OSCPortIn(6666);
            OSCListener listener = new OSCListener() {
                public void acceptMessage(Date time, OSCMessage message) {
                    omuc.oscMessages.add(message);
                }
            };
            receiver.addListener("/testin", listener);
            receiver.startListening();
        }
        catch (SocketException e) {
            Log.d("OSCSendInitalisation", "Socket exception error!");
        }
    }

    public static int getNumMessages()
    {
        return omuc.getNumMessages();
    }

    public static void clearMessages()
    {
        omuc.oscMessages.clear();
    }

    public static String getMessageAddress(int index)
    {
        return omuc.getMessageAddress(index);
    }

    public static float getMessageFloat(int index)
    {
        return omuc.getMessageFloatArg(index);
    }
}
