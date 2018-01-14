//
//  YeahMobiRewardedVideoCustomEvent.m
//  Pods
//
//  Created by 최치웅 on 2017. 9. 19..
//
//

#import "YeahMobiRewardedVideoCustomEvent.h"
#import <CTSDK/CTSDK.h>

#import "MoPub.h"
#import "MPLogging.h"
#import "MPRewardedVideoReward.h"

@interface YeahMobiRewardedVideoCustomEvent()<CTRewardVideoDelegate>

@property(nonatomic,assign)BOOL canPlay;

@end


@implementation YeahMobiRewardedVideoCustomEvent

- (void)requestRewardedVideoWithCustomEventInfo:(NSDictionary *)info
{
    NSString *slotid = [info objectForKey:@"slotid"];
    CTService* manager = [CTService shareManager];
    [manager loadRequestGetCTSDKConfigBySlot_id:slotid];
    
    [manager loadRewardVideoWithSlotId:slotid delegate:self];
}

- (void)presentRewardedVideoFromViewController:(UIViewController *)viewController
{
    [[CTService shareManager] showRewardVideo];
}

- (BOOL)hasAdAvailable
{
    return self.canPlay;
}

- (void)CTRewardVideoLoadSuccess
{
    self.canPlay = YES;
    if ([self.delegate respondsToSelector:@selector(trackImpression)]) {
        [self.delegate trackImpression];
    }
    if ([self.delegate respondsToSelector:@selector(rewardedVideoDidLoadAdForCustomEvent:)]) {
        [self.delegate rewardedVideoDidLoadAdForCustomEvent:self];
    }
}

/**
 *  CTRewardVideo bigin playing Ad
 **/
- (void)CTRewardVideoDidStartPlaying
{
}

/**
 *  CTRewardVideo playing Ad finish
 **/
- (void)CTRewardVideoDidFinishPlaying
{
    [self.delegate rewardedVideoShouldRewardUserForCustomEvent:self reward:[[MPRewardedVideoReward alloc] initWithCurrencyAmount:@(kMPRewardedVideoRewardCurrencyAmountUnspecified)]];
}

/**
 *  CTRewardVideo Click Ads
 **/
- (void)CTRewardVideoDidClickRewardAd
{
    if ([self.delegate respondsToSelector:@selector(trackClick)]) {
        [self.delegate trackClick];
    }
}

/**
 * CTRewardVideo will leave Application
 **/
- (void)CTRewardVideoWillLeaveApplication
{
    if ([self.delegate respondsToSelector:@selector(rewardedVideoWillDisappearForCustomEvent:)]) {
        [self.delegate rewardedVideoWillDisappearForCustomEvent:self];
    }
    
    if ([self.delegate respondsToSelector:@selector(rewardedVideoDidDisappearForCustomEvent:)]) {
        [self.delegate rewardedVideoDidDisappearForCustomEvent:self];
    }
    
    
}

/**
 *  CTRewardVideo jump AppStroe failed
 **/
- (void)CTRewardVideoJumpfailed
{
    NSLog(@"CTRewardVideoJumpfailed--------------");
}

/**
 *  CTRewardVideo loading failed
 **/
- (void)CTRewardVideoLoadingFailed:(NSError *)error
{
    if ([self.delegate respondsToSelector:@selector(rewardedVideoDidFailToPlayForCustomEvent:error:)]) {
        [self.delegate rewardedVideoDidFailToPlayForCustomEvent:self error:error];
    }
}

- (void)CTRewardVideoClosed
{
    if ([self.delegate respondsToSelector:@selector(rewardedVideoWillDisappearForCustomEvent:)]) {
        [self.delegate rewardedVideoWillDisappearForCustomEvent:self];
    }
    
    if ([self.delegate respondsToSelector:@selector(rewardedVideoDidDisappearForCustomEvent:)]) {
        [self.delegate rewardedVideoDidDisappearForCustomEvent:self];
    }
}

- (void)showAd
{
    if ([self.delegate respondsToSelector:@selector(rewardedVideoWillAppearForCustomEvent:)]) {
        [self.delegate rewardedVideoWillAppearForCustomEvent:self];
    }
    
    if ([self.delegate respondsToSelector:@selector(rewardedVideoDidAppearForCustomEvent:)]) {
        [self.delegate rewardedVideoDidAppearForCustomEvent:self];
    }
}
@end
