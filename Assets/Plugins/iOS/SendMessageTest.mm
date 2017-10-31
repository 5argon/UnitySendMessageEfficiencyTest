
#import "SendMessageTest.h"

@implementation SendMessageTest 

@end


extern "C" {

    void _NativeSendMessage(const char* gameObjectName, const char* methodName, const char* message) {
        UnitySendMessage(gameObjectName, methodName, message);
    }

    typedef void (*ActionDelegate)(const char* message);
    void _PointerHackTest(ActionDelegate actionDelegate) {
         actionDelegate("hey");
    }
    
}



