//
//  IOSBridge.m
//  
//
//  Created by Dave Yang on 6/28/15.
//
//

#import "IOSBridge.h"

@implementation Delegate

-(id)init
{
    return self;
}
-(void) alertView:(UIAlertView*)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    NSString *inStr=[NSString stringWithFormat:@"%d",(int)buttonIndex];
    const char *cString=[inStr cStringUsingEncoding:NSASCIIStringEncoding];
    UnitySendMessage("Menu", "UserFeedBack", cString);
}

@end
static Delegate* delegateObject;
extern "C"
{
    void _AddNotification(const char* title,
                          const char* body,
                          const char* cancelLabel,
                          const char* firstLabel,
                          const char* secondLabel)
    {
        if(delegateObject==nil)
        {
            delegateObject=[[Delegate alloc] init];
        }
        UIAlertView *alert=[[UIAlertView alloc]
                            initWithTitle:[NSString stringWithUTF8String:title]
                            message:[NSString stringWithUTF8String:body] delegate:delegateObject
                            cancelButtonTitle:[NSString stringWithUTF8String:cancelLabel]
                            otherButtonTitles:[NSString stringWithUTF8String:firstLabel],
                            [NSString stringWithUTF8String:secondLabel],nil];
        [alert show];
                            
    }
}