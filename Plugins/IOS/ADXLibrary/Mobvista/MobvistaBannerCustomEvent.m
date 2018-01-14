//
//  MobvistaBannerCustomEvent.m
//  YelloJihachul
//
//  Created by 최치웅 on 2017. 2. 13..
//  Copyright © 2017년 Imagedrome. All rights reserved.
//

#import <MVSDK/MVSDK.h>
#import <MVSDK/MVNativeAdManager.h>

#import "MobvistaBannerCustomEvent.h"

#import "MPInstanceProvider.h"
#import "MPLogging.h"

@interface MobvistaBannerCustomEvent() <MVNativeAdManagerDelegate>

@property (nonatomic, readwrite, strong) MVNativeAdManager *mvNativeAdManager;
@end

static BOOL initialized = NO;
@implementation MobvistaBannerCustomEvent

- (void)requestAdWithSize:(CGSize)size customEventInfo:(NSDictionary *)info
{
    if(!CGSizeEqualToSize(size, CGSizeMake(320, 50))) {
        MPLogError(@"Invalid size for Mobvista banner ad");
        [self.delegate bannerCustomEvent:self didFailToLoadAdWithError:nil];
        return;
    }
    
    NSString *appId = [NSString stringWithFormat:@"%@",[info objectForKey:@"app_id"]];//nonnull
    NSString *appKey = [NSString stringWithFormat:@"%@",[info objectForKey:@"app_key"]];//nonnull
    NSString *unitId = [NSString stringWithFormat:@"%@",[info objectForKey:@"adunit_id"]];//nonnull
    NSString *placementID = [NSString stringWithFormat:@"%@",[info objectForKey:@"placementId"]];
    
    MVAdCategory adCategory =[info objectForKey:@"adCategory"]? [[info objectForKey:@"adCategory"] integerValue]:MVAD_CATEGORY_ALL;
    
    MVAdTemplateType templateType = [info objectForKey:@"templateType"] ?[[info objectForKey:@"templateType"] integerValue]:MVAD_TEMPLATE_BIG_IMAGE;
    
    MVAdTemplateType reqNum = [info objectForKey:@"reqNum"] ?[[info objectForKey:@"reqNum"] integerValue]:1;
    
    if (!initialized) {
        initialized = YES;
        [[MVSDK sharedInstance] setAppID:appId ApiKey:appKey];
    }
    
    _mvNativeAdManager = [[MVNativeAdManager alloc] initWithUnitID:unitId fbPlacementId:placementID supportedTemplates:@[[MVTemplate templateWithType:templateType adsNum:reqNum]] autoCacheImage:YES adCategory:adCategory presentingViewController:nil];
    
    _mvNativeAdManager.delegate = self;
    [_mvNativeAdManager loadAds];
}

- (void)nativeAdsLoaded:(nullable NSArray *)nativeAds {
    if (nativeAds.count > 0) {
        MVCampaign *campaign = nativeAds[0];
        
        UIView * view = [[UIView alloc] initWithFrame:CGRectMake(0, 0, 320, 50)];
        UIImageView * iconView = [[UIImageView alloc] initWithFrame:CGRectMake(0, 0, 50, 50)];
        
        [campaign loadIconUrlAsyncWithBlock:^(UIImage *image) {
            [iconView setImage:image];
        }];
        
        CGSize size = [campaign.adCall sizeWithAttributes:@{NSFontAttributeName:[UIFont boldSystemFontOfSize:12]}];
        
        UIButton * ctaButton = [[UIButton alloc] initWithFrame:CGRectMake(320 - size.width, 0, size.width, 50)];
        ctaButton.titleLabel.font = [UIFont boldSystemFontOfSize:12];
        
        [ctaButton setTitle:campaign.adCall forState:UIControlStateNormal];
        
        ctaButton.backgroundColor = [UIColor colorWithRed:0.76 green:0.07 blue:0.07 alpha:1.0];
        ctaButton.frame = CGRectMake(ctaButton.frame.origin.x - 12, 0, ctaButton.frame.size.width + 12, 50);
        
        UITextView * titleTextView = [[UITextView alloc] initWithFrame:CGRectMake(55, 0, 245 - ctaButton.frame.size.width, 30)];
        titleTextView.text = campaign.appName;
        titleTextView.font = [UIFont boldSystemFontOfSize:12];
        titleTextView.textContainer.maximumNumberOfLines = 1;
        titleTextView.textContainer.lineBreakMode = NSLineBreakByTruncatingTail;
        
        UITextView * sponsoredTextView = [[UITextView alloc] initWithFrame:CGRectMake(55, 20, 245 - ctaButton.frame.size.width, 20)];
        sponsoredTextView.text = @"Sponsored";
        sponsoredTextView.font = [UIFont systemFontOfSize:10];
        
        [view addSubview:iconView];
        [view addSubview:ctaButton];
        [view addSubview:titleTextView];
        [view addSubview:sponsoredTextView];
        
        [_mvNativeAdManager registerViewForInteraction:view withCampaign:campaign];
        
        [self.delegate bannerCustomEvent:self didLoadAd:view];
    } else {
        [self.delegate bannerCustomEvent:self didFailToLoadAdWithError:nil];
    }
}

- (void)nativeAdsFailedToLoadWithError:(nonnull NSError *)error {
    [self.delegate bannerCustomEvent:self didFailToLoadAdWithError:error];
}

- (void)nativeAdClickUrlWillStartToJump:(nonnull NSURL *)clickUrl {
    [self.delegate bannerCustomEventWillBeginAction:self];
}

- (void)nativeAdClickUrlDidEndJump:(nullable NSURL *)finalUrl
                             error:(nullable NSError *)error {
    [self.delegate bannerCustomEventDidFinishAction:self];
}

- (UIViewController *)viewControllerForPresentingModalView
{
    return [self.delegate viewControllerForPresentingModalView];
}

@end
