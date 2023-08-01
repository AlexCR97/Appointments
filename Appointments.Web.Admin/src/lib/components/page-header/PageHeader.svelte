<script lang="ts">
	import { goto, afterNavigate } from '$app/navigation';
	import { Button } from '$lib/components/forms';
	import { pageActions, pageTitle, pageSubtitle } from './PageHeaderStore';

	let previousPath: string | undefined;

	afterNavigate(({ from }) => {
		previousPath = from?.url.pathname;
	});

	function onBackClick(_: MouseEvent, previousPath: string | undefined): void {
		if (previousPath) {
			goto(previousPath, { replaceState: true });
		}
	}

	function onHrefClick(_: MouseEvent, href: string | undefined): void {
		if (href) {
			goto(href);
		}
	}

	function onButtonClick(_: MouseEvent, click: (() => void) | undefined): void {
		if (click) {
			click();
			return;
		}
	}
</script>

<div class="d-flex align-items-center w-100 border-bottom px-3 py-2">
	<div>
		<h5 class="m-0">{$pageTitle}</h5>
		{#if $pageSubtitle}
			<p class="m-0">{$pageSubtitle}</p>
		{/if}
	</div>

	<div class="ms-auto d-flex">
		{#each $pageActions as action}
			{#if action.back === true && previousPath !== undefined}
				<!-- TODO Figure out why the back buttons is rendered event if there is not a previousPath-->
				<Button
					class="ms-2"
					label={action.label}
					variant={action.variant}
					on:click={(e) => onBackClick(e, previousPath)}
				/>
			{:else if action.href !== undefined}
				<Button
					class="ms-2"
					label={action.label}
					variant={action.variant}
					on:click={(e) => onHrefClick(e, action.href)}
				/>
			{:else}
				<Button
					class="ms-2"
					label={action.label}
					form={action.form}
					type={action.type}
					variant={action.variant}
					on:click={(e) => onButtonClick(e, action.click)}
				/>
			{/if}
		{/each}
	</div>
</div>
