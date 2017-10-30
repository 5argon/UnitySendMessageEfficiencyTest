
#import "SendMessageTest.h"

@implementation SendMessageTest 

@end


extern "C" {

    void _NativeSendMessage(const char* gameObjectName, const char* methodName, const char* message) {
        UnitySendMessage(gameObjectName, methodName, message);
    }
    
}



