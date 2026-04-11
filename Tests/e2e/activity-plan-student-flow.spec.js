/**
 * E2E 测试: Phase 6 学生 AI 活动发布与进入流程
 *
 * 依赖外部测试数据：
 * - PLAYWRIGHT_TEACHER_USERNAME / PLAYWRIGHT_TEACHER_PASSWORD
 * - PLAYWRIGHT_STUDENT_SNUM / PLAYWRIGHT_STUDENT_PASSWORD
 * - PLAYWRIGHT_ACTIVITY_PLAN_COURSE_ID
 */
import { test, expect } from '@playwright/test';
import { teacherLogin, studentLogin, TEST_ACCOUNTS } from './auth.helper.js';

const activityPlanTopic = process.env.PLAYWRIGHT_ACTIVITY_PLAN_TOPIC || 'Phase 6 自动化验证活动';

const draftPayload = {
  teachingGoals: ['理解分数含义', '能结合情境表达分数'],
  activitySteps: [
    {
      title: '情境导入',
      minutes: '5分钟',
      teacherAction: '展示分蛋糕图片并提问',
      studentAction: '观察图片并回答',
      interactionMethod: '提问交流',
      resourceSuggestion: '蛋糕图片或实物卡片',
      assessmentCheck: '根据学生表述判断是否理解平均分',
    },
  ],
  resources: ['分数卡片'],
  assessment: ['观察学生是否能正确说出二分之一'],
  teacherReminder: '注意让学生先说生活例子。',
};

const selectedSections = [
  'teachingGoals',
  'activitySteps',
  'resources',
  'assessment',
  'teacherReminder',
];

test.describe('Phase 6 学生活动发布链路', () => {
  test.skip(
    !TEST_ACCOUNTS.teacher || !TEST_ACCOUNTS.student || !TEST_ACCOUNTS.activityPlanCourseId,
    '需要通过环境变量提供教师账号、学生账号和课程 ID。'
  );

  test('教师发布后学生可从任务页进入 showmission 并看到引导内容', async ({ browser }) => {
    const teacherContext = await browser.newContext();
    const teacherPage = await teacherContext.newPage();

    await teacherLogin(
      teacherPage,
      TEST_ACCOUNTS.teacher.username,
      TEST_ACCOUNTS.teacher.password,
    );

    await teacherPage.goto(`/teacher/courseedit.aspx?cid=${TEST_ACCOUNTS.activityPlanCourseId}`);
    await teacherPage.waitForLoadState('domcontentloaded');

    const publishResponse = await teacherPage.evaluate(async ({ cid, topic, currentDraft, sections }) => {
      const params = new URLSearchParams();
      params.set('action', 'activityPlanPublish');
      params.set('cid', cid);
      params.set('topic', topic);
      params.set('publishToStudents', 'true');
      params.set('selectedSections', JSON.stringify(sections));
      params.set('currentDraft', JSON.stringify(currentDraft));

      const response = await fetch('aiprovider_api.ashx', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: params.toString(),
      });

      return {
        status: response.status,
        body: await response.text(),
      };
    }, {
      cid: TEST_ACCOUNTS.activityPlanCourseId,
      topic: activityPlanTopic,
      currentDraft: draftPayload,
      sections: selectedSections,
    });

    expect(publishResponse.status).toBe(200);

    const publishJson = JSON.parse(publishResponse.body || '{}');
    expect(publishJson.success).toBe(true);
    expect(publishJson.data?.publishedToStudents).toBe(true);
    expect(publishJson.data?.listMenuId).toBeTruthy();
    expect(publishJson.data?.missionTitle).toContain(activityPlanTopic);

    await teacherContext.close();

    const studentContext = await browser.newContext();
    const studentPage = await studentContext.newPage();

    await studentLogin(
      studentPage,
      TEST_ACCOUNTS.student.snum,
      TEST_ACCOUNTS.student.password,
    );

    await studentPage.goto(`/student/showmission.aspx?lid=${publishJson.data.listMenuId}`);
    await studentPage.waitForLoadState('domcontentloaded');

    await expect(studentPage).toHaveURL(new RegExp(`showmission\\.aspx\\?lid=${publishJson.data.listMenuId}`));
    await expect(studentPage.locator(`a[href*="showmission.aspx?lid=${publishJson.data.listMenuId}"]`).first()).toBeVisible();
    await expect(studentPage.locator('#showcontent')).toContainText('学习建议');
    await expect(studentPage.locator('#showcontent')).toContainText('学习目标');
    await expect(studentPage.locator('#showcontent')).toContainText('活动说明');
    await expect(studentPage.locator('#showcontent')).toContainText('任务步骤');
    await expect(studentPage.locator('#showcontent')).toContainText('活动主题');
    await expect(studentPage.locator('#showcontent')).toContainText(activityPlanTopic);
    await expect(studentPage.locator('#showcontent')).toContainText('作品提交区');

    await studentContext.close();
  });
});
